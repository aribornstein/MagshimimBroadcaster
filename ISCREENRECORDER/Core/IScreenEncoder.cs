using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IScreenEncoder
    {
        /* 
        * Encode a screen to a byte array.
        * Parameters: The screen to encode.
        * Return Value: A byte array of the encoded data.
        */
        byte[] Encode(IScreen screen);

        /* 
        * Get a screen from an encoded packet.
        * Parameters: A byte array of the encoded data.
        * Return Value: The screen that is decoded.
        */
        IScreen Decode(byte[] packet);

    }
}
