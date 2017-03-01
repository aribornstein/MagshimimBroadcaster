using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static void runExe(string run)
        {
            Process p = new Process();
            p.StartInfo.FileName = run;
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.FileName = "1.exe";
            //startInfo.Arguments = run;
            //p.StartInfo = startInfo;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
        }

        static void Main(string[] args)
        {
            string run = "C:\\Users\\User\\Desktop\\SCI\\sendVidNotWebSockets\\ConsoleApplication1\\ConsoleApplication1\\obj\\Debug\\1.exe";
            string vid = "C:\\Users\\User\\Desktop\\SCI\\sendVidNotWebSockets\\ConsoleApplication1\\ConsoleApplication1\\bin\\Debug\\record.avi";
            int PORT = 11000;

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, PORT);
            //Socket k = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket k = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                k.Connect("127.0.0.1", PORT);
                runExe(run);
                k.SendFile(vid, null, null, TransmitFileOptions.UseDefaultWorkerThread);
                k.Shutdown(SocketShutdown.Both);
                k.Close();

                /*string s1 = String.Format("This is text data that preceds the file.{0}", Environment.NewLine);
                byte[] preBuf = Encoding.ASCII.GetBytes(s1);
                string s2 = String.Format("This is text data that will follow the file.{0}", Environment.NewLine);
                byte[] postBuf = Encoding.ASCII.GetBytes(s2);
                Console.WriteLine("Sending {0} with buffers to the host.{1}", vid, Environment.NewLine);
                c.SendFile(vid, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);*/
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //Console.ReadKey();
        }
    }
}
