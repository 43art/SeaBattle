using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SeaBattle {
    public partial class GameStart : Form {
        StartField startfield = new StartField(0, 0);
        StartField opponentfield = new StartField(0, 600);
        int i = 0;
        int sds = 4;    //single-deck   ship
        int dds = 3;    //double-deck   ship
        int tds = 2;    //three-deck    ship
        int fds = 1;    //four-deck     ship
        bool is_your_turn = false;
        TcpClient client = null;

        public GameStart(bool nIs_your_turn, TcpClient nClient) {
            InitializeComponent();
            Set_labels();
            is_your_turn = nIs_your_turn;
            client = nClient;
            if (is_your_turn) {
                this.Text = "Первый игрок";
                WhoTurnLabel.Text = "Сейчас ваш ход";
            }
            else {
                this.Text = "Второй игрок";
                WhoTurnLabel.Text = "Сейчас ход противника";
            }
        }

        private void Test_Set_Ships(Graphics g) {
            // Четырехпалубный корабль
            startfield.getCell(0, 0).setState(2);
            startfield.getCell(0, 1).setState(2);
            startfield.getCell(0, 2).setState(2);
            startfield.getCell(0, 3).setState(2);
            fds = 0;
            // Трёхпалубные корабли
            startfield.getCell(2, 0).setState(2);
            startfield.getCell(2, 1).setState(2);
            startfield.getCell(2, 2).setState(2);
            startfield.getCell(4, 0).setState(2);
            startfield.getCell(4, 1).setState(2);
            startfield.getCell(4, 2).setState(2);
            tds = 0;
            // Двухпалубные корабли
            startfield.getCell(0, 5).setState(2);
            startfield.getCell(0, 6).setState(2);
            startfield.getCell(0, 8).setState(2);
            startfield.getCell(0, 9).setState(2);
            startfield.getCell(2, 4).setState(2);
            startfield.getCell(2, 5).setState(2);
            dds = 0;
            // Однопалубные корабли
            startfield.getCell(2, 7).setState(2);
            startfield.getCell(2, 9).setState(2);
            startfield.getCell(4, 4).setState(2);
            startfield.getCell(4, 6).setState(2);
            sds = 0;
            Set_labels();
            startfield.draw(g);
        }

        private void GameStart_Paint(object sender, PaintEventArgs e) {
            Graphics g = CreateGraphics();
            startfield.draw(g);
            if (button1.Visible)
                Test_Set_Ships(g);
            opponentfield.draw(g);
        }

        // Устанавливает лейблы с остатками кораблей на форме
        private void Set_labels() {
            label1.Text = "Однопалубных кораблей осталось " + sds.ToString();
            label2.Text = "Двухпалубных кораблей осталось " + dds.ToString();
            label3.Text = "Трехпалубных кораблей осталось " + tds.ToString();
            label4.Text = "Четырехпалубных кораблей осталось " + fds.ToString();
        }

        // Сбрасывает теги используемых кнопок на стандартные
        public void Reset() {
            sds = 4; dds = 3; tds = 2; fds = 1;
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    startfield.getCell(i, j).setTag(0);
                }
            }
        }

        private void setShip(MouseEventArgs e) {
            Cell cell = null;

            if (e.Button.ToString() == "Left") {
                cell = startfield.getCellByCoordinate(e.X, e.Y);
                cell.setState(2);
                Graphics g = CreateGraphics();

                Checkships(cell);
                cell.draw(g);
            }
            if (e.Button.ToString() == "Right") {
                cell = startfield.getCellByCoordinate(e.X, e.Y);
                cell.setState(0);
                Graphics g = CreateGraphics();
                cell.draw(g);
            }
            ShipRemaining();
        }

        // Обработка событий мыши на клетки игрового поля
        private void GameStart_MouseClick(object sender, MouseEventArgs e) {
            if ((e.X < 498) && (e.Y < 498)) {
                if (!button1.Visible)
                    return;
                setShip(e);
                return;
            }

            Cell cell = null;
            if ((e.X <= 1100) && (e.X > 600) && (e.Y < 498)) {
                if (!is_your_turn)
                    return;

                if (e.Button.ToString() == "Left") {
                    cell = opponentfield.getCellByCoordinate(e.X - 600, e.Y);
                    string msg = "shot:" + cell.getX().ToString() + '_' + cell.getY().ToString();
                    sendData(msg);
                    string result = "";
                    try {
                        result = receiveData();
                    }
                    catch {
                        MessageBox.Show("Проблемы с сервером");
                        this.Close();
                    }

                    Graphics g = CreateGraphics();
                    if (result == "true") {
                        cell.setState(5);
                        cell.draw(g);
                        if (opponentfield.checkFinishOpponent()) {
                            MessageBox.Show("Победа " + this.Text.ToString() + '!');
                            this.Close();
                        }
                    }
                    else {
                        cell.setState(1);
                        cell.draw(g);
                        is_your_turn = false;
                        WhoTurnLabel.Text = "Сейчас ход противника";
                        WhoTurnLabel.Refresh();
                        // На поражение чекаем в waitForShot
                        Thread clientThread = new Thread(new ThreadStart(waitForShot));
                        clientThread.Start();
                    }
                }
            }
        }

        // Проверка соседствующих клеток на наличие кораблей
        private void Checkships(Cell cell) {
            Cell cell1;
            Graphics g = CreateGraphics();

            int mainX = cell.getX();
            int mainY = cell.getY();

            for (int i = mainX - 50; i <= mainX + 50; i += 100) {
                if (i <= 0) continue;
                if (i >= 498) continue;
                for (int j = mainY - 50; j <= mainY + 50; j += 100) {
                    if (j <= 0) continue;
                    if ((i == mainX) && (j == mainY)) continue;
                    if (j >= 498) continue;

                    cell1 = startfield.getCellByCoordinate(i, j);
                    if (cell1.getState() == 2) {
                        MessageBox.Show("Боевые корабли соседствуют друг с другом!");
                        cell.setState(0);
                    }
                }
            }
        }

        // Проверка остатка кораблей для расстановки
        private void ShipRemaining() {
            Reset();

            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Cell cell = startfield.getCell(i, j);
                    if ((cell.getState() == 2) && (cell.getTag() != 1)) {
                        // Случай вправо
                        cell.setTag(1);
                        Cell cell1 = startfield.getCell(i + 1, j);

                        if ((cell1 != null) && (cell1.getState() == 2)) {
                            cell1.setTag(1);
                            Cell cell2 = startfield.getCell(i + 2, j);

                            if ((cell2 != null) && (cell2.getState() == 2)) {
                                cell2.setTag(1);
                                Cell cell3 = startfield.getCell(i + 3, j);
                                if ((cell3 != null) && (cell3.getState() == 2)) {
                                    cell3.setTag(1);
                                    fds--;
                                }
                                else tds--;
                            }
                            else dds--;
                        }
                        else {
                            // Случай вниз
                            cell1 = startfield.getCell(i, j + 1);
                            if ((cell1 != null) && cell1.getState() == 2) {
                                cell1.setTag(1);
                                Cell cell2 = startfield.getCell(i, j + 2);

                                if ((cell2 != null) && (cell2.getState() == 2)) {
                                    cell2.setTag(1);
                                    Cell cell3 = startfield.getCell(i, j + 3);
                                    if ((cell3 != null) && cell3.getState() == 2) {
                                        cell3.setTag(1);
                                        fds--;
                                    }
                                    else tds--;
                                }
                                else dds--;

                            }
                            else {
                                sds--;
                            }
                        }

                    }
                }
            }
            Set_labels();
        }

        // Ожидание хода противника. Прослушивание данных от сервера
        private void waitForShot() {
            string shot = "";
            try {
                shot = receiveData();
            }
            catch {
                MessageBox.Show("Проблемы с сервером");
                this.Close();
            }
            string[] parameters = shot.Split(':');
            string[] indexes = parameters[1].Split('_');
            int i = Convert.ToInt32(indexes[0]);
            int j = Convert.ToInt32(indexes[1]);
            Cell cell = startfield.getCellByCoordinate(i - 600, j);
            int state = cell.getState();
            Graphics g = CreateGraphics();

            if (state == 2) {
                cell.setState(3);
                cell.draw(g);
                sendData("true");
                if (startfield.checkFinishSelf()) {
                    MessageBox.Show("Поражение " + this.Text.ToString());
                    this.Close();
                }
                waitForShot();
            }
            else {
                cell.setState(1);
                cell.draw(g);
                //this.Refresh();
                sendData("false");
                is_your_turn = true;
                WhoTurnLabel.Text = "Сейчас ваш ход";
                this.Refresh();
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e) {
            ShipRemaining();

            if ((sds == 0) && (dds == 0) && (tds == 0) && (fds == 0)) {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                button1.Visible = false;
                if (!is_your_turn) {
                    Thread clientThread = new Thread(new ThreadStart(waitForShot));
                    clientThread.Start();
                }
            }
            else {
                MessageBox.Show("Осталось расположить " + sds.ToString() + " однопалубных кораблей\n" +
                           "Осталось расположить " + dds.ToString() + " двухпалубных кораблей\n" +
                           "Осталось расположить " + tds.ToString() + " треххпалубных кораблей\n" +
                           "Осталось расположить " + fds.ToString() + " четырехпалубных кораблей\n");

                // Возвращение исходных значений переменных
                Reset();
            }
        }

        // Утилита отправки данных
        public void sendData(string msg) {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.GetStream().Write(data, 0, data.Length);
        }

        public string receiveData() {
            // Получаем ответ
            byte[] data = new byte[1024]; // Буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            NetworkStream stream = client.GetStream();
            int bytes = 0;
            do {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            return builder.ToString();
        }
    }
}