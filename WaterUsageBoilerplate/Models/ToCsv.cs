using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace WaterUsageBoilerplate.Models
{
    public class ToCsv //backing fields for objectss that csvhelper wants
    {
        public float Frequency1 { get; set; }
        public float Frequency2 { get; set; }
        public float Frequency3 { get; set; }
        public float Frequency4 { get; set; }
        public float Frequency5 { get; set; }
        public float Magnitude1 { get; set; }
        public float Magnitude2 { get; set; }
        public float Magnitude3 { get; set; }
        public float Magnitude4 { get; set; }
        public float Magnitude5 { get; set; }
    }
}
