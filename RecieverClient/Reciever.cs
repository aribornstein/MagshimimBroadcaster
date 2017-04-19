using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using VideoEncoding;


namespace RecieverClient
{
    class Reciever : IReciever
    {

        Socket s;
        H264Encoder decoder;
        int w;
        int h;

        //Default constructor.
        public Reciever()
        {
            //create listener socket to server
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint IPE = new IPEndPoint(IP, 4321);
            s.Connect(IPE);
            BinaryFormatter serializer = new BinaryFormatter();
            using (var network = new NetworkStream(s))
            {
                w = (int) serializer.Deserialize(network);
                h = (int)serializer.Deserialize(network);
            }
            decoder = new H264Encoder(w, h);

        }

        /* 
        * Get a bitmap from the socket.
        * Parameters: None.
        * Return Value: IScreen of type BitmapScreen.
        */
        public IScreen Receive()
        {

            BinaryFormatter serializer = new BinaryFormatter();
            using (var network = new NetworkStream(s))
            {
                byte[] map = (byte[])serializer.Deserialize(network);
                RgbScreen im = decoder.Decode(map) as RgbScreen;
                BitmapScreen screen = new BitmapScreen();
                screen.screenRepresentation = im.GetAsBitmap(w, h);
                return screen;
            }
        }
    }
}
