using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterUsageBoilerplate.Models
{
    public class DBSCANNAH //Density analyzer to extrapolate water usage
    {
        private List<top5Frequencies>? top5Frequencies;
        private double? waterUsage;
        private string? typeOfUsage;
        private Tuple<List<float>, List<float>> data;

        public double DBSCAN(List<top5Frequencies> top5Frequencies, double waterUsage, string typeOfUsage)
        {
            this.top5Frequencies = top5Frequencies;
            this.waterUsage = waterUsage;
            this.typeOfUsage = typeOfUsage;

            int instancesOfWaterUsage = 0;
            double waterRate = (typeOfUsage == "Shower") ? 0.37 : 0.08; //change this
            List<float> Freq = new(), Mag = new();

            data = new(Freq, Mag);
            foreach (top5Frequencies i in top5Frequencies)
            {
                if (i.Frequencies.Sum() / i.Frequencies.Count <= 1550 && i.Magnitudes.Sum() / i.Magnitudes.Count <= 28)
                {
                    Freq.Add((i.Frequencies.Sum() / i.Frequencies.Count) / 1559);
                    Mag.Add((i.Magnitudes.Sum() / i.Magnitudes.Count) / 28);
                }
            }

            for (int i = 0; i < data.Item1.Count - data.Item1.Count % 10; i += 10)
            {
                int clusteroids = CheckRegion(i);
                instancesOfWaterUsage += (clusteroids >= 1) ? 1 : 0;
                clusteroids = 0;
            }

            waterUsage = waterRate * (double)instancesOfWaterUsage;
            return waterUsage;
        }

        private int CheckRegion(int index) //Check 1.2 second increments for if there's water usage
        {
            int neighbors = 0, clusteroids = 0;

            for (int i = index; i < index + 10; i++)
            {
                for (int j = index; j < index + 10; j++)
                {
                    if (j == i) continue;
                    neighbors += (EuclideanDistanceFormula(data.Item1[i], data.Item2[i],
                    data.Item1[j], data.Item2[j]) < 0.03) ? 1 : 0;
                }
                clusteroids += (neighbors >= 1 && neighbors <= 5) ? 1 : 0;
                neighbors = 0;
            }
            return clusteroids;
        }

        private float EuclideanDistanceFormula(float X1, float Y1, float X2, float Y2)
        {
            float sum = (float)(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
            return (float)Math.Sqrt(sum);
        }
    }
}
