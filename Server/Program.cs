using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server {

    class Program {
        const int port = 6969; // порт для прослушивания подключений
        static ClientConnection client1;
        static ClientConnection client2;

        static void Main(string[] args) {
            TcpListener listener = null;
            try {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(localAddr, port);
                listener.Start();
                Console.WriteLine("Wait for connection...");

                while (true) {
                    TcpClient client = listener.AcceptTcpClient();
                    if (client1 != null && client2 != null)
                        break;
                    Console.WriteLine("Client connected!");

                    if (client1 == null) {
                        client1 = new ClientConnection(client, 1);
                        Thread clientThread = new Thread(new ThreadStart(client1.process));
                        clientThread.Start();
                    }
                    else {
                        client2 = new ClientConnection(client, 2, client1);
                        client1.otherPlayer = client2;
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
