using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server {
    // Класс обработки соединения каждого игрока
    class ClientConnection {
        TcpClient client;
        int id; // Номер игрока - первый/второй
        public ClientConnection otherPlayer = null; // ссылка на другого игрока, чтобы отправлять ему данные

        public ClientConnection(TcpClient nclient, int nid) {
            this.client = nclient;
            this.id = nid;
        }

        public ClientConnection(TcpClient nclient, int nid, ClientConnection nOtherPlayer) : this(nclient, nid) {
            this.otherPlayer = nOtherPlayer;
        }

        // Утилита отправки данных
        private void sendData(string msg, NetworkStream stream) {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }

        // Метод непосредственной обработки входящего сообщения
        private void processRequest(string message, NetworkStream stream) {
            string[] parameters = message.Split(':');
            string answer;
            if (parameters[0] == "connect") {
                answer = (id == 1) ? "true" : "false";
                sendData(answer, stream);
                return;
            }
            else {
                sendData(message, otherPlayer.getStream());
            }
        }

        // Общий метод получения сообщений
        public void process() {
            NetworkStream stream = null;
            try {
                stream = client.GetStream();
                byte[] data = new byte[1024];
                while (true) {
                    StringBuilder sb = new StringBuilder();
                    int bytes = 0;
                    do {
                        bytes = stream.Read(data, 0, data.Length);
                        sb.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = sb.ToString();
                    Console.WriteLine("Client" + id.ToString() + ": " + message);
                    processRequest(message, stream);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            finally {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        // Метод получения потока записи
        public NetworkStream getStream() {
            return client.GetStream();
        }
    }
}
