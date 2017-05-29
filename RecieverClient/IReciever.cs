using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.IO;

namespace RecieverClient
{
    interface IReciever
    {
        /* 
        * Get a screen object from the network.
        * Parameters: None.
        * Return Value: MemoryStream - sound or screen data
        */
        MemoryStream Receive();
    }
}
