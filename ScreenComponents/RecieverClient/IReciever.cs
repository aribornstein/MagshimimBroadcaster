using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;


namespace RecieverClient
{
    interface IReciever
    {
        /* 
        * Get a screen object from the network.
        * Parameters: None.
        * Return Value: IScreen - the representation of the screen.
        */
        IScreen Receive();
    }
}
