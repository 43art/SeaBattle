using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace SeaBattle
{
    public partial class GameStart : Form
    {
        StartField startfield = new StartField(0,0);
        StartField opponentfield = new StartField (0, 600);
        int i = 0;
        int sds = 4;    //single-deck   ship
        int dds = 3;    //double-deck   ship
        int tds = 2;    //three-deck    ship
        int fds = 1;    //four-deck     ship
        bool is_your_turn = false;
        TcpClient client = null;

        public GameStart(bool nIs_your_turn, TcpClient nClient) 
        {
            InitializeComponent();
            Set_labels();
            is_your_turn = nIs_your_turn;
            client = nClient;
            if (is_your_turn)
                this.Text = "Первый игрок";
            else
                this.Text = "Второй игрок";
            //Test_Set_Ships();
        }

        // TODO: Delete k huyam
        private void Test_Set_Ships() {
            //startfield.getCell()
        }

        private void GameStart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            startfield.draw(g);
            opponentfield.draw(g);
        }

        //устанавливает лейблы с остатками кораблей на форме
        private void Set_labels()
        {
            label1.Text = "Однопалубных кораблей осталось " + sds.ToString();
            label2.Text = "Двухпалубных кораблей осталось " + dds.ToString();
            label3.Text = "Трехпалубных кораблей осталось " + tds.ToString();
            label4.Text = "Четырехпалубных кораблей осталось " + fds.ToString();
        }

        //сбрасывает системные переменные
        public void Reset()
        {
            sds = 4; dds = 3; tds = 2; fds = 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    startfield.getCell(i, j).setTag(0);
                }
            }
        }

        private void GameStart_MouseClick(object sender, MouseEventArgs e)
        {
            Cell cell = null;
            if ((e.X < 498) && (e.Y < 498))
            {
                if (e.Button.ToString() == "Left")
                {
                    cell = startfield.getCellByCoordinate(e.X, e.Y);
                    cell.setState(2);
                    Graphics g = CreateGraphics();

                    Checkships(cell);
                    cell.draw(g);
                }
           

                if (e.Button.ToString() == "Right")
                {
                    cell = startfield.getCellByCoordinate(e.X, e.Y);
                    cell.setState(0);
                    Graphics g = CreateGraphics();
                    cell.draw(g);
                }
                
            }
            //Проверка на остаток кораблей
            ShipRemaining();

            if ((e.X <= 1100) && (e.X > 600) && (e.Y < 498))
            {
                if (!is_your_turn)
                    return;

                if (e.Button.ToString() == "Left")
                {
                    cell = opponentfield.getCellByCoordinate(e.X - 600, e.Y);
                    string msg = "shot:" + cell.getX().ToString() + '_' + cell.getY().ToString();
                    sendData(msg);
                    string result = receiveData();

                    Graphics g = CreateGraphics();
                    if (result == "true") {
                        cell.setState(5);
                        cell.draw(g);
                        Checkships(cell);
                    }
                    else
                    {
                        cell.setState(1);
                        is_your_turn = false;
                        cell.draw(g);
                        Checkships(cell);
                        waitForShot();
                    }
                }


                if (e.Button.ToString() == "Right")
                {
                    cell = opponentfield.getCellByCoordinate(e.X - 600, e.Y);
                    cell.setState(0);
                    Graphics g = CreateGraphics();
                    cell.draw(g);
                }

            }
        }

        //Проверка соседствующих клеток на наличие кораблей
        private void Checkships(Cell cell)
        {
            Cell cell1;
            Graphics g = CreateGraphics();

            int mainX = cell.getX();
            int mainY = cell.getY();

            for (int i = mainX - 50; i <= mainX + 50; i += 100)
            {
                if (i <= 0) continue;
                if (i >= 498) continue;
                for (int j = mainY - 50; j <= mainY + 50; j += 100)
                {
                    if (j <= 0) continue;
                    if ((i == mainX) && (j == mainY)) continue;
                    if (j >= 498) continue;

                    cell1 = startfield.getCellByCoordinate(i, j);
                    if (cell1.getState() == 2)
                    {
                        MessageBox.Show("Боевые корабли соседствуют друг с другом!");
                        cell.setState(0);
                    }
                }
            }
        }

        //Остаток кораблей
        private void ShipRemaining()
        {
            Reset();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell cell = startfield.getCell(i, j);
                    if ((cell.getState() == 2) && (cell.getTag() != 1))
                    {
                        //случай вправо
                        cell.setTag(1);
                        Cell cell1 = startfield.getCell(i + 1, j);

                        if ((cell1 != null) && (cell1.getState() == 2))
                        {
                            cell1.setTag(1);
                            Cell cell2 = startfield.getCell(i + 2, j);

                            if ((cell2 != null) && (cell2.getState() == 2))
                            {
                                cell2.setTag(1);
                                Cell cell3 = startfield.getCell(i + 3, j);
                                if ((cell3 != null) && (cell3.getState() == 2))
                                {
                                    cell3.setTag(1);
                                    fds--;
                                }
                                else tds--;
                            }
                            else dds--;
                        }
                        else
                        {
                            //случай вниз
                            cell1 = startfield.getCell(i, j + 1);
                            if ((cell1 != null) && cell1.getState() == 2)
                            {
                                cell1.setTag(1);
                                Cell cell2 = startfield.getCell(i, j + 2);

                                if ((cell2 != null) && (cell2.getState() == 2))
                                {
                                    cell2.setTag(1);
                                    Cell cell3 = startfield.getCell(i, j + 3);
                                    if ((cell3 != null) && cell3.getState() == 2)
                                    {
                                        cell3.setTag(1);
                                        fds--;
                                    }
                                    else tds--;
                                }
                                else dds--;

                            }
                            else
                            {
                                sds--;
                            }
                        }

                    }
                }
            }
            Set_labels();
        }

        private void waitForShot() {
            string shot = receiveData();
            string[] parameters = shot.Split(':');
            string[] indexes = parameters[1].Split('_');
            int i = Convert.ToInt32(indexes[0]);
            int j = Convert.ToInt32(indexes[1]);
            Cell cell = startfield.getCellByCoordinate(i-600, j);
            int state = cell.getState();

            if (state == 2)
            {
                cell.setState(3);
                sendData("true");
            }
            else
            {
                sendData("false");
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            ShipRemaining();

            if ((sds == 0) && (dds == 0) && (tds == 0) && (fds == 0))
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                button1.Visible = false;
                if (!is_your_turn)
                    waitForShot();
            }
            else 
            {
                MessageBox.Show("Осталось расположить " + sds.ToString() + " однопалубных кораблей\n" +
                           "Осталось расположить " + dds.ToString() + " двухпалубных кораблей\n" +
                           "Осталось расположить " + tds.ToString() + " треххпалубных кораблей\n" +
                           "Осталось расположить " + fds.ToString() + " четырехпалубных кораблей\n");

                //Возвращение исходных значений переменных
                Reset();
            }
        }

        // Утилита отправки данных
        public void sendData(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.GetStream().Write(data, 0, data.Length);
        }

        public string receiveData()
        {
            // получаем ответ
            byte[] data = new byte[1024]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = client.GetStream().Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (client.GetStream().DataAvailable);
            return builder.ToString();
        }

    }
}
