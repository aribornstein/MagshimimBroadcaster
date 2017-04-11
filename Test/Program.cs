using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Runtime.InteropServices;
using Encoding1;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Encoding1.Encoding encode = new Encoding1.Encoding(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            
            while (true)
            {
                byte[] stuff = encode.GetEncoding(new byte[] { 1, 2, 3, 4 });
                if (stuff.Length > 0)
                {
                    string hex = BitConverter.ToString(stuff);
                    Console.WriteLine(hex);
                }
            }
            Console.ReadKey();

        }
    }
}
