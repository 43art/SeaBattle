﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server {
    class ClientConnection {
        TcpClient client;
        int id = 0;
        public ClientConnection otherPlayer = null;

        public ClientConnection(TcpClient nclient, int nid) {
            this.client = nclient;
            this.id = nid;
        }

        public ClientConnection(TcpClient nclient, int nid, ClientConnection nOtherPlayer) : this(nclient, nid) {
            this.otherPlayer = nOtherPlayer;
        }

        private void sendAnswer(string answer, NetworkStream stream) {
            byte[] data = Encoding.Unicode.GetBytes(answer);
            stream.Write(data, 0, data.Length);
        }

        private void processRequest(string message, NetworkStream stream) {
            string[] parameters = message.Split(':');
            string answer;
            if (parameters[0] == "connect") {
                answer = (id == 1) ? "true" : "false";
                sendAnswer(answer, stream);
                return;
            }
            else {
                sendAnswer(parameters[0], otherPlayer.getStream());
            }
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

        public NetworkStream getStream() {
            return client.GetStream();
        }
    }
}
