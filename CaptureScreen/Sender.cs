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
using System.IO;

namespace CaptureScreen
{
    class Sender : ISender
    {

        TcpClient client;
        NetworkStream network;
        H264Encoder encoder;
        BinaryFormatter serializer = new BinaryFormatter();
        
        //Constructor that takes an address and a port and the width of the video.
        public Sender(IPAddress address, short port, int w, int h)
        {
            client = new TcpClient("localhost", 3333);
            network = client.GetStream();
            encoder = new H264Encoder(w, h); //Uses a bitmap encoder.

            serializer.Serialize(network, w);
            serializer.Serialize(network, h);
            
        }

        /* 
        * Send a screen to the server.
        * Parameters: The screen.
        * Return Value: None.
        */
        public void Send(IScreen screen)
        {
            byte[] byteArray = encoder.Encode(screen);
            if (byteArray.Length > 0)
            {
                serializer.Serialize(network, byteArray);
            }
        }

        /* 
        * Send sound to the server.
        * Parameters: sound.
        * Return Value: None.
        */
        public void Send1(MemoryStream ms)
        {
            serializer.Serialize(network, ms.ToArray());
        }
    }
}
