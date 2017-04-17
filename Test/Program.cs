using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Runtime.InteropServices;
using VideoEncoding;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            while (true)
            {
                VideoEncoding.Encoding encode = new VideoEncoding.Encoding(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                byte[] stuff;
                for(int i = 0; i < 200; i++)
                {
                    stuff = encode.GetEncoding(new byte[] { 1, 2, 3, 4 });
                    /*if (stuff.Length > 0)
                    {
                        string hex = BitConverter.ToString(stuff);
                        Console.WriteLine(hex);
                    }
                    */
                }
                Console.WriteLine("press.");
                Console.ReadKey();
            }
        }
    }
}
