using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Core;
using VideoEncoding;
using System.Drawing.Imaging;

namespace CaptureScreen
{
    class Recorder : IRecorder
    {
        int w;
        int h;
        VideoEncoding.Encoding encoder;
        Bitmap pic;
        Rectangle rect;
        byte[] im;

        public Recorder()
        {
            w = Screen.PrimaryScreen.Bounds.Width;
            h = Screen.PrimaryScreen.Bounds.Height;
            encoder = new VideoEncoding.Encoding(w, h);
            pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        }

        /* 
        * Capture a bitmap from the screen.
        * Parameters: None.
        * Return Value: A BitmapScreen.
        */
        public IScreen Capture()
        {

            im = encoder.CaptureBitmap();
            if (im.Length > 0)
            {
                ArgbScreen data = new ArgbScreen();
                data.screenRepresentation = im;
                pic = data.GetAsBitmap(w, h);
            }
            BitmapScreen screen = new BitmapScreen();
            screen.screenRepresentation = pic;
            return screen;
        }

        /*
        Get the actual pixel data, without the bitmap format.
        */
        public byte[] GetBitmapData()
        {
            return im;
        }
    }
    
}


//alternative implementation.
/*
Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); // this is the bitmap to push into your stream and process on your client
Graphics g = Graphics.FromImage(bm);
g.CopyFromScreen(0, 0, 0, 0, bm.Size);
BitmapScreen screen = new BitmapScreen();
screen.screenRepresentation = bm;
return screen;
*/
