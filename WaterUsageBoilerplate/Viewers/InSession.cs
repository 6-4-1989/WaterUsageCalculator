using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterUsageBoilerplate.Models;
using NAudio.Wave;
using System.Data;
using System.Numerics;
using ScottPlot;

namespace WaterUsageBoilerplate.Viewers
{
    public class InSession
    {
        public int SampleRate { get; set; }
        public WaveFileReader Reader { get; set; }
        public AudioStuffs AudioStuff { get; set; }
        public WaveFormat WavFormat { get; set; }
        public FourierProcessing? FFTProcessing { get; set; }
        public float[]? AudioData { get; set; }
        public List<top5Frequencies>? topFrequencies { get; set; }
        public Messaging messaging { get; set; }
        public VisualizeData visualize { get; set; }

        public InSession(string FilePath)
        {
            visualize = new VisualizeData();
            Reader = new WaveFileReader(@$"{FilePath}");
            WavFormat = Reader.WaveFormat;
            SampleRate = WavFormat.SampleRate;
            
            AudioStuff = new AudioStuffs()
            {
                reader = Reader,
                waveFormat = WavFormat
            };
            messaging = new Messaging();
            messaging.CurrentMessage = AudioStuff.FileMessenger();

            if (messaging.CurrentMessage == "Now calculating water usage...")
            {
                AudioData = AudioStuff.StoreAudioData();

                FourierProcessing fftProcessing = new FourierProcessing()
                {
                    meineAudioData = AudioData,
                    sampleRate = SampleRate
                };

                topFrequencies = fftProcessing.GetIncrements();
                messaging.CurrentMessage = "Complete";
                visualize.PlotData(topFrequencies);
            }
        }
    }
}
