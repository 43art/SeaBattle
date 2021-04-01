using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server {

    class Program {
        const string IP = "127.0.0.1";
        const int port = 6969; // порт для прослушивания подключений
        static ClientConnection client1;    // игрок1
        static ClientConnection client2;    // игрок2

        static void Main(string[] args) {
            TcpListener listener = null;
            try {
                IPAddress localAddr = IPAddress.Parse(IP);
                listener = new TcpListener(localAddr, port);
                listener.Start();
                Console.WriteLine("Wait for connection...");

                while (true) {
                    TcpClient client = listener.AcceptTcpClient();
                    if (client1 != null && client2 != null) 
                        // Если уже есть 2 игрока, большк никого не пускаем
                        continue;

                    Console.WriteLine("Client connected!");

                    if (client1 == null) {
                        client1 = new ClientConnection(client, 1);
                        // Запускаем отдельный поток для работы с первым игроком
                        Thread clientThread = new Thread(new ThreadStart(client1.process));
                        clientThread.Start();
                    }
                    else {
                        client2 = new ClientConnection(client, 2, client1);
                        client1.otherPlayer = client2;
                        // Запускаем отдельный поток для работы со вторым игроком
                        Thread clientThread = new Thread(new ThreadStart(client2.process));
                        clientThread.Start();
                    }
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
