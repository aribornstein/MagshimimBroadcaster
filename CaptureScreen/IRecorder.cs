using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;


namespace CaptureScreen
{
    interface IRecorder
    {
        /* 
        * Capture a screen object.
        * Parameters: None.
        * Return Value: A screen object.
        */
        IScreen Capture();
    }
}
