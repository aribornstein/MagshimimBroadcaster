using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Core;

namespace CaptureScreen
{
    class Recorder : IRecorder
    {
        /* 
        * Capture a bitmap from the screen.
        * Parameters: None.
        * Return Value: A BitmapScreen.
        */
        public IScreen Capture()
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); // this is the bitmap to push into your stream and process on your client
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            BitmapScreen screen = new BitmapScreen();
            screen.screenRepresentation = bm;
            return screen;
        }



    }
}
