using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
    class Networking
    {
        short _port;
        TcpListener serverSocket;

        public Networking(short port)
        {
            _port = port;
        }

        /*
        Start listening on port.
        */
        public void Listen()
        {
            serverSocket = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), _port);
            serverSocket.Start();
            Console.WriteLine("Server started.");
        }

        /*
        Start accpeting connections from clients.
        */
        public void AcceptConnections()
        {
            while (true)
            {
                TcpClient clientSocket = serverSocket.AcceptTcpClient();
                Thread t = new Thread(() => HandleClient(clientSocket));
                t.Start();
            }
        }

        /*
       Handle communications with a certain client. 
       Parameters: clientSocket - a tcp client to handle.
       Return Value: None.
       */
        private void HandleClient(TcpClient clientSocket)
        {
            try
            {
                NetworkStream stream = clientSocket.GetStream();
                using (FileStream file = new FileStream("stream.avi", FileMode.Create))
                {
                    stream.CopyTo(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
