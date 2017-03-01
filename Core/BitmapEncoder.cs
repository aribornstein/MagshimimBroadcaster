using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BitmapEncoder : IScreenEncoder
    {

        ImageConverter converter = new ImageConverter();

        /* 
        * Get the decoded bitmap from the packet.
        * Parameters: the encoded packet (an exact byte to byte replica of the bitmap).
        * Return Value: IScreen of type BitmapScreen.
        */
        public IScreen Decode(byte[] packet)
        {
            MemoryStream ms = new MemoryStream(packet);
            ms.Write(packet, 0, packet.Length);
            BitmapScreen screen = new BitmapScreen();
            screen.screenRepresentation = new System.Drawing.Bitmap(ms); 
            return screen;

        }


        /* 
        * Get the encoded packet from the bitmap.
        * Parameters: The bitmap .
        * Return Value: An encoded packet (an exact byte to byte replica of the bitmap).
        */
        public byte[] Encode(IScreen screen)
        {
            Bitmap bm = screen.screenRepresentation as Bitmap;
            return (byte[])converter.ConvertTo(bm, typeof(byte[]));
        }
    }
}
