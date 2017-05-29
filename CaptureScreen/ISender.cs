using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CaptureScreen
{
    interface ISender
    {
        /* 
        * Send a screen object to the server.
        * Parameters: The screen.
        * Return Value: None.
        */
        void Send(IScreen screen);

    }
}
