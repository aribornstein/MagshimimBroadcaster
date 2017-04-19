using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ArgbScreen : IScreen
    {
        byte[] im;
        public object screenRepresentation
        {
            get { return im; }
            set { im = value as byte[]; }
        }

        public Bitmap GetAsBitmap(int w, int h)
        {
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, w, h);
            if (im.Length > 0)
            {
                BitmapData bmpData = pic.LockBits(rect, ImageLockMode.WriteOnly, pic.PixelFormat);
                int padding = bmpData.Stride - 3 * w;
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;

                    for (int i = 0; i < im.Length; i += 4)
                    {
                        ptr[i + 3] = 255;
                        ptr[i + 2] = im[i + 2];
                        ptr[i + 1] = im[i + 1];
                        ptr[i] = im[i];

                    }
                }

                pic.UnlockBits(bmpData);
            }
            return pic;
        }
    }
}
