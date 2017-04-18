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
        Sender serverSender;
        bool count;


        public Form1()
        {
            InitializeComponent();
            count = false;
            recorder = new Recorder();
            //serverSender = new Sender(Dns.GetHostEntry("localhost").AddressList[0], 3333);
        }

        void capture()
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            VideoEncoding.Encoding encoder = new VideoEncoding.Encoding(w, h);
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, w, h);

            using (FileStream f = new FileStream("stuff.h264", FileMode.Create))
            {
                while (true)
                {

                    byte[] encoding = encoder.GetEncoding(new byte[] { 1, 2, 3, 4 });
                    if (encoding.Length > 0)
                    {

                        f.Write(encoding, 0, encoding.Length);
                        byte[] im = encoder.GetDecoding(encoding);
                        if (im.Length > 0)
                        {
                            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.WriteOnly, pic.PixelFormat);
                            int padding = bmpData.Stride - 3 * w;

                            unsafe
                            {
                                byte* ptr = (byte*)bmpData.Scan0;
                                for(int i = 0; i < im.Length; i += 3)
                                {
                                    ptr[i+2] = im[i];
                                    ptr[i + 1] = im[i + 1];
                                    ptr[i] = im[i + 2];
                                }
                               
                            }

                            pic.UnlockBits(bmpData);

                            pictureBox.Image = (Image)pic.Clone();

                        }
                    }
                }
                //BitmapScreen screen = recorder.Capture() as BitmapScreen; //get a bitmap
                //pictureBox.Image = (Image) (screen.screenRepresentation as Bitmap).Clone(); //display it.
                //serverSender.Send(screen); //send to the server.
            }
        }

        private void Record_Click(object sender, EventArgs e)
        {
            if(!count)
            {
                Thread t = new Thread(capture);
                t.Start();
                count = true;
            }
        }

    }
}
