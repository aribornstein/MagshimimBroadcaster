using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using VideoEncoding;
using System.IO;
using System.Drawing.Imaging;

namespace CaptureScreen
{
    public partial class Form1 : Form
    {
        
        Recorder recorder;
        AudioC audioRecorder;
        Sender serverSender;
        bool count;


        public Form1()
        {
            InitializeComponent();
            count = false;
            recorder = new Recorder();
            serverSender = new Sender(Dns.GetHostEntry("localhost").AddressList[0], 3333, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            audioRecorder = new AudioC();
        }

        void capture()
        {

            while (true)
            {

                BitmapScreen screen = recorder.Capture() as BitmapScreen; //get a bitmap

                Bitmap bitmap = screen.screenRepresentation as Bitmap;
                pictureBox.Image = (Image) bitmap.Clone(); //display it.

                //convert to ArgbScreen.
                ArgbScreen data = new ArgbScreen();
                data.screenRepresentation = recorder.GetBitmapData();

                serverSender.Send(data); //send to the server.
            }
        }

        void capture1()
        {
            Queue<MemoryStream> q = new Queue<MemoryStream>(), q1 = new Queue<MemoryStream>();
            MemoryStream ms;
            while (true)
            {
                audioRecorder.RecordAudio(q, q1);
                if (q.Count > 0)
                {
                    serverSender.Send1(q.Dequeue());
                }
                if (q1.Count > 0)
                {
                    serverSender.Send1(q1.Dequeue());
                }
            }
        }

        private void Record_Click(object sender, EventArgs e)
        {
            if (!count)
            {
                Thread t = new Thread(capture);
                Thread t1 = new Thread(capture1);
                t.Start();
                t1.Start();
                count = true;
            }
        }
    }
}
