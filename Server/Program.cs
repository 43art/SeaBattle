using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server {
    class ClientConnection {
        TcpClient client;

        public ClientConnection(TcpClient nclient) {
            this.client = nclient;
        }

        private void processRequest(string message, NetworkStream stream) {
            // TODO:
            byte[] data = Encoding.Unicode.GetBytes("SERVER ACCEPTED: " + message);
            stream.Write(data, 0, data.Length); ;
        }

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
                    Console.WriteLine(message);
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
    }

    class Program {
        const int port = 6969; // порт для прослушивания подключений

        static void Main(string[] args) {
            TcpListener listener = null;
            try {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(localAddr, port);
                listener.Start();
                Console.WriteLine("Wait for connection...");

                while (true) {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientConnection cc = new ClientConnection(client);

                    Console.WriteLine("Client connected!");
                    Thread clientThread = new Thread(new ThreadStart(cc.process));
                    clientThread.Start();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            finally {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
