using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



namespace Core
{
    public class H264Encoder : IScreenEncoder
    {
        [DllImport(@"..\x64\Debug\EncodingDLL.dll")]
        private static extern void Initialize(int width, int height);

        [DllImport(@"..\x64\Debug\EncodingDLL.dll")]
        private static extern IntPtr GetEncoding(IntPtr im);



        public IScreen Decode(byte[] packet)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode(IScreen screen)
        {
            
            return null;
        }

    }
}
