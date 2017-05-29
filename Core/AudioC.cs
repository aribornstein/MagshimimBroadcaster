using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using CSCore.Streams;
using System.Media;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using System.Threading;

namespace Core
{
    public class AudioC
    {
        //records the audio of both the microphone and computer into different memorystreams
        //and pushes each to a respective queue
        public void RecordAudio(Queue<MemoryStream> q, Queue<MemoryStream> q1)
        {
            MemoryStream ms1;
            WasapiCapture capture1;
            WaveWriter w1;
            MemoryStream ms;
            WasapiLoopbackCapture capture;
            WaveWriter w;
            capture = new WasapiLoopbackCapture();
            capture.Initialize();
            ms = new MemoryStream();
            w = new WaveWriter(ms, capture.WaveFormat);
            capture1 = new WasapiCapture();
            capture1.Initialize();
            ms1 = new MemoryStream();
            w1 = new WaveWriter(ms1, capture.WaveFormat);
            capture.DataAvailable += (s, capData) =>
            {
                w.Write(capData.Data, capData.Offset, capData.ByteCount);
            };
            capture1.DataAvailable += (s, capData) =>
            {
                w1.Write(capData.Data, capData.Offset, capData.ByteCount);
            };
            capture.Start();
            capture1.Start();
            System.Threading.Thread.Sleep(1000);
            if (w != null && capture != null && w1 != null && capture1 != null)
            {
                capture.Stop();
                capture1.Stop();
            }
            ms = msToSuccess(ms);
            ms1 = msToSuccess(ms1);
            if (ms.Length > 44)
            {
                q.Enqueue(ms);
            }
            if (ms1.Length > 44)
            {
                q1.Enqueue(ms1);
            }
            ms.Dispose();
            w.Dispose();
            capture.Dispose();
            ms1.Dispose();
            w1.Dispose();
            capture1.Dispose();
        }
        //plays a queue of audio whenever it updates
        public void PlayThat(MemoryStream ms)
        {
            Audio p = new Audio();
            p.Play(ms, Microsoft.VisualBasic.AudioPlayMode.WaitToComplete);
        }
        //testing function
        public void msToFile(MemoryStream m, string name)
        {
            FileStream f = new FileStream(name, FileMode.Create, System.IO.FileAccess.Write);
            m.WriteTo(f);
        }
        //fixes memorystream and returns a new one, so it can be playable
        private MemoryStream msToSuccess(MemoryStream m)
        {
            byte[] initialWavByte = new byte[m.ToArray().Length - 44];
            System.Buffer.BlockCopy(m.ToArray(), 44, initialWavByte, 0, m.ToArray().Length - 44);
            string[] header = HeaderMaker(m);
            byte[][] x = new byte[11][];
            int size = 0;
            for (int i = 0; i < 11; i++)
            {
                x[i] = ByteMaker(header[i]);
                size += x[i].Length;
            }
            byte[] r = new byte[size + initialWavByte.Length];
            size = 0;
            for (int i = 0; i < 11; i++)
            {
                System.Buffer.BlockCopy(x[i], 0, r, size, x[i].Length);
                size += x[i].Length;
            }
            System.Buffer.BlockCopy(initialWavByte, 0, r, size, initialWavByte.Length);
            return new MemoryStream(r);
        }
        //makes a header for the memorystream
        private string[] HeaderMaker(MemoryStream m)
        {
            string changes = (m.Length - 44).ToString("X");
            string[] cs = new string[3];
            string[] header = new string[11];
            header[0] = "52494646";
            header[1] = "24000000";
            header[2] = "57415645";
            header[3] = "666D7420";
            header[4] = "10000000";
            header[5] = "03000200";
            header[6] = "80BB0000";
            header[7] = "00DC0500";
            header[8] = "08002000";
            header[9] = "64617461";
            header[10] = "00000000";
            if (changes.Length != 0)
            {
                if (changes.Length == 1)
                {
                    cs[1] = "00";
                    cs[2] = "00";
                }
                else if (changes.Length == 2)
                {
                    cs[1] = "00";
                    cs[2] = "00";
                }
                else if (changes.Length == 3)
                {
                    cs[1] = "0" + changes.Substring(0, 1);
                    cs[2] = "00";
                }
                else if (changes.Length == 4)
                {
                    cs[1] = changes.Substring(0, 2);
                    cs[2] = "00";
                }
                else if (changes.Length == 5)
                {
                    cs[1] = changes.Substring(1, 2);
                    cs[2] = "0" + changes.Substring(0, 1);
                }
                else if (changes.Length == 6)
                {
                    cs[1] = changes.Substring(2, 2);
                    cs[2] = changes.Substring(0, 2);
                }
                changes = cs[1] + cs[2];
            }
            header[1] = header[1].Insert(2, changes);
            header[1] = header[1].Remove((int)changes.Length + 2);
            header[10] = header[10].Insert(2, changes);
            header[10] = header[10].Remove((int)changes.Length + 2);
            header[1] = header[1].Insert((int)changes.Length + 2, "00");
            header[10] = header[10].Insert((int)changes.Length + 2, "00");
            return header;
        }
        //returns a byte array of a hex string
        private byte[] ByteMaker(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }
    }
}
