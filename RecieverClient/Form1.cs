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

            while (true)
            {
                //Get an image and display it.
                BitmapScreen screen = (BitmapScreen)receiver.Receive();
                pictureBox.Image = (Bitmap) screen.screenRepresentation;
            }

        }

    }
}
