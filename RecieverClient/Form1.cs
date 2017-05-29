using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using VideoEncoding;


namespace RecieverClient
{
    public partial class Form1 : Form
    {

        Reciever receiver;
        bool count;

        public Form1()
        {
            InitializeComponent();
            count = false;
            receiver = new Reciever();
        }

        private void Recieve_Click(object sender, EventArgs e)
        {
            if(!count)
            {
                Thread t = new Thread(recieve);
                t.Start();
                count = true;
            }
        }

        void recieve() {
            MemoryStream ms;
            int w = receiver.getW(), h = receiver.getH();
            H264Encoder decoder = receiver.getD();
            AudioC player = new AudioC();
            while (true)
            {
                ms = receiver.Receive();
                if (ms.ToArray()[0] != Convert.ToByte('R'))//WAV header first letter is R
                { 
                    RgbScreen im = decoder.Decode(ms.ToArray()) as RgbScreen;
                    BitmapScreen screen = new BitmapScreen();
                    screen.screenRepresentation = im.GetAsBitmap(w, h);
                    pictureBox.Image = (Bitmap)screen.screenRepresentation;
                }
                else
                {
                    player.PlayThat(ms);
                }
            }

        }

    }
}
