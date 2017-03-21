using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Core;


namespace CaptureScreen
{
    class Sender : ISender
    {

        TcpClient client;
        NetworkStream network;
        BitmapEncoder encoder;
        BinaryFormatter serializer = new BinaryFormatter();
        
        //Constructor that takes an address and a port.
        public Sender(IPAddress address, short port)
        {
            client = new TcpClient("localhost", 3333);
            network = client.GetStream();
            encoder = new BitmapEncoder(); //Uses a bitmap encoder.
            
            
        }

        /* 
        * Send a screen to the server.
        * Parameters: The screen.
        * Return Value: None.
        */
        public void Send(IScreen screen)
        {
            byte[] byteArray = encoder.Encode(screen);
            serializer.Serialize(network, byteArray);
        }
    }
}
