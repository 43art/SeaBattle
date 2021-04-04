using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace SeaBattle {
    public partial class Connect : Form {
        TcpClient client = null;
        NetworkStream stream = null;

        public Connect() {
            InitializeComponent();
        }

        private void gamestart_button_Click(object sender, EventArgs e) {
            try {
                string ip = IPTextBox.Text;
                int port = Convert.ToInt32(PortTextBox.Text);
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                sendData("connect");
                string turn = receiveData();
                bool is_your_turn = (turn == "true");
                GameStart newForm = new GameStart(is_your_turn, client);
                this.Hide();
                newForm.FormClosed += (s, args) => this.Close();
                newForm.Show();
                
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewForm_FormClosed(object sender, FormClosedEventArgs e) {
            throw new NotImplementedException();
        }

        // Утилита отправки данных
        public void sendData(string msg) {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }

        public string receiveData() {
            // получаем ответ
            byte[] data = new byte[1024]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
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
