using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.WPF;
using ScottPlot;
using HarfBuzzSharp;

namespace WaterUsageBoilerplate.Models
{
    public class VisualizeData //Place frequency and magnitude data into plot as a test to determine clustering algorithm
    {
        public WpfPlot myPlot { get; } = new WpfPlot();

        public void PlotData(List<top5Frequencies> dataList)
        {
            List<float> frequencies = new List<float>();
            List<float> magnitudes = new List<float>();

            foreach (top5Frequencies i in dataList)
            { 
                if (i.Frequencies.Sum() / i.Frequencies.Count <= 1550 && i.Magnitudes.Sum() / i.Magnitudes.Count <= 28)
                {
                    frequencies.Add((i.Frequencies.Sum() / i.Frequencies.Count) / 1550);
                    magnitudes.Add((i.Magnitudes.Sum() / i.Magnitudes.Count) / 28); 
                }
            }
            float[] initializer = frequencies.ToArray();
            float[] initializer2 = magnitudes.ToArray();
            myPlot.Plot.Add.ScatterPoints(initializer, initializer2);
        }
    }
}
