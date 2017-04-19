using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StreamServer
{
    class Server
    {

        IPAddress localhost = IPAddress.Loopback;
        IPEndPoint IPE;

        Socket sListen = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream,
                                    ProtocolType.Tcp);

        List<Socket> clients = new List<Socket>();

        const int BUFFER_SIZE = 2048;

        /* 
        * Start the server.
        * Parameters: None.
        * Return Value: None.
        */
        public void startServer()
        {

            //port for recording clients.
            TcpListener recieveServer = new TcpListener(localhost, 3333);
            recieveServer.Start();


            //send server
            IPE = new IPEndPoint(localhost, 4321);
            sListen.Bind(IPE);
            sListen.Listen(2);

            //start a thread to listen for connections.
            Thread t = new Thread(ListenForConnections);
            t.Start();


            Socket socketToHandleRequest = recieveServer.AcceptSocket(); //accept client request.
            NetworkStream network = new NetworkStream(socketToHandleRequest);

            byte[] buff = new byte[BUFFER_SIZE];
            while (true)
            {

                lock (this) //to handle multiple client call issue
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    object map =  serializer.Deserialize(network); //deserialize from the network stream.
                    SendContinuation(map);
                }
                socketToHandleRequest = null; // assign null to release memeory.
            }

        }


        /* 
        * Listen for incoming connection of receivers.
        * Parameters: None.
        * Return Value: None.
        */
        void ListenForConnections()
        {
            while(true)
            {
                Socket clientSocket;
                clientSocket = sListen.Accept();
                lock (clients)
                {
                    clients.Add(clientSocket);
                }
            }
        }


        /* 
        * Send a byte array to the clients of the current stream.
        * Parameters: the packet to send.
        * Return Value: None.
        */
        public void SendContinuation(object bytes)
        {
            lock(clients) //Make sure no new client are inserted.
            {
                foreach (var client in clients)
                {
                    try
                    {
                        using (var network = new NetworkStream(client))
                        {
                            BinaryFormatter serializer = new BinaryFormatter();
                            serializer.Serialize(network, bytes); //serialize to the socket of the client.
                        }
                    }
                    catch (Exception e)
                    {
                        //If a sending operation fails disconnect from the client.
                        Console.WriteLine(e.ToString());
                        clients.Remove(client);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Server s = new Server();
            s.startServer();
            
        }
    }
}
