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
using System.Globalization;

namespace WaterUsageBoilerplate.Viewers
{
    public class InSession //nesting horror I'm sorry
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
        public DBSCANNAH dbScan { get; set; }

        public InSession(string FilePath, string usageSelected)
        {
            visualize = new VisualizeData();
            dbScan = new();
            messaging = new Messaging();
            messaging.WaterUsageActivity = usageSelected;

            if (usageSelected == null)
            {
                messaging.CurrentMessage = "Please enter what you're going to do via the dropdown";
            }
            else
            {
                Reader = new WaveFileReader(@$"{FilePath}");
                WavFormat = Reader.WaveFormat;
                SampleRate = WavFormat.SampleRate;

                AudioStuff = new AudioStuffs()
                {
                    reader = Reader,
                    waveFormat = WavFormat
                };
                messaging.CurrentMessage = AudioStuff.FileMessenger();
            }

            if (messaging.CurrentMessage == "Now calculating water usage...")
            {
                AudioData = AudioStuff.StoreAudioData();

                FourierProcessing fftProcessing = new FourierProcessing()
                {
                    meineAudioData = AudioData,
                    sampleRate = SampleRate
                };

                topFrequencies = fftProcessing.GetIncrements(); 
                messaging.TotalWaterUsage = dbScan.DBSCAN(topFrequencies, 
                    double.Parse(messaging.TotalWaterUsage, CultureInfo.InvariantCulture), messaging.WaterUsageActivity).ToString();
                messaging.CurrentMessage = "Complete";
                visualize.PlotData(topFrequencies);
            }
        }
    }
}
