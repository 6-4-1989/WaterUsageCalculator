using System;
using System.Linq;
using System.Diagnostics;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;

namespace WaterUsageBoilerplate.Models
{
    public class AudioStuffs
    {
        //backing fields
        public WaveFileReader reader { get; set; }
        public WaveFormat waveFormat { get; set; }

        public string FileMessenger()
        {

            if (waveFormat.BitsPerSample != 32)
            {
                return "Must be 32 bits PCM"; //ensure 32 bits format for consistency
            }

            if (waveFormat.Channels != 1)
            {
                return "Must be mono"; //screw interleaving
            }
            return "Now calculating water usage...";
        }

        public float[] StoreAudioData()
        {
            byte[] tempBuffer = new byte[reader.Length];
            //utilize stream to read bytes to array
            int samplesRead = reader.Read(tempBuffer, 0, tempBuffer.Length);
            int[] buffer = new int[samplesRead / 4]; //8 bits to 32 bit storage
            Buffer.BlockCopy(tempBuffer, 0, buffer, 0, samplesRead);

            float[] floatBuffer = new float[buffer.Length];
            floatBuffer = buffer.Select(x => x / (float)Int32.MaxValue).ToArray();

            return floatBuffer;
        }
    }
}
