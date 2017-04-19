using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using VideoEncoding;
using System.Windows.Forms;



namespace Core
{
    public class H264Encoder : IScreenEncoder
    {
        
        VideoEncoding.Encoding encoder;

        public H264Encoder(int w, int h)
        {
            encoder = new VideoEncoding.Encoding(w, h);
        }


        /*
        Decode the packet and return a 24-bit bitmap.
        Parameters: the packet.
        Return Value: the decoded screen if the decoding was successful. otherwise, an empty array.
        */
        public IScreen Decode(byte[] packet)
        {
            RgbScreen screen = new RgbScreen();
            screen.screenRepresentation = encoder.GetDecoding(packet);
            return screen;
        }


        /*
        Encode a screen where each pixel is 4 bytes.
        Parameters: the screen.
        Return value: The encoded array.
        */
        public byte[] Encode(IScreen screen)
        {
            byte[] im = (byte[]) (screen.screenRepresentation);
            return encoder.GetEncoding(im);
        }

    }
}
