using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
    public class RgbScreen : IScreen
    {
        byte[] im;
        public object screenRepresentation
        {
            get { return im; }
            set { im = value as byte[]; }
        }

        public Bitmap GetAsBitmap(int w, int h)
        {
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, w, h);
            if (im.Length > 0)
            {
                BitmapData bmpData = pic.LockBits(rect, ImageLockMode.WriteOnly, pic.PixelFormat);
                int padding = bmpData.Stride - 3 * w;
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            ptr[0] = im[(j + w * i) * 3 + 2];
                            ptr[1] = im[(j + w * i) * 3 + 1];
                            ptr[2] = im[(j + w * i) * 3 + 0];
                            ptr += 3;
                        }
                        ptr += padding;
                    }
                }
                pic.UnlockBits(bmpData);
            }
            return pic;
        }
    }
}
