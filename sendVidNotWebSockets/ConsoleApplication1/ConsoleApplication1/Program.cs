using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class Program
    {
        static void runExe(string run)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = run;
            p.StartInfo = startInfo;
            p.Start();
        }

        static void Main(string[] args)
        {
            string run = "1.exe";
            string vid = "record.avi";
            int PORT = 11000;

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, PORT);

            Socket c = new Socket(ipAddr.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            //Socket k = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                /*k.Connect(ipEndPoint);
                runExe(run);
                k.SendFile(vid);
                k.Shutdown(SocketShutdown.Both);
                k.Close();*/

                /*string s1 = String.Format("This is text data that preceds the file.{0}", Environment.NewLine);
                byte[] preBuf = Encoding.ASCII.GetBytes(s1);
                string s2 = String.Format("This is text data that will follow the file.{0}", Environment.NewLine);
                byte[] postBuf = Encoding.ASCII.GetBytes(s2);
                Console.WriteLine("Sending {0} with buffers to the host.{1}", vid, Environment.NewLine);
                c.SendFile(vid, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);*/

                runExe(run);
                c.SendFile(vid);
                c.Shutdown(SocketShutdown.Both);
                c.Close();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
        }
    }
}
