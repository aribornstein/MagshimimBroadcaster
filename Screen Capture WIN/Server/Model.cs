using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Model
    {
        Networking net;

        public Model(short port)
        {
            net = new Networking(port);
        }

        /*
        Begin running the server.
        */
        public void Run()
        {
            net.Listen();
            net.AcceptConnections();
        }
    }
}
