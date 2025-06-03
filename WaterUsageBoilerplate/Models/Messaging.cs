using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WaterUsageBoilerplate.Viewers;

namespace WaterUsageBoilerplate.Models
{
    public class Messaging : PropertyChangedClass //Handles some random crud I swear I can't anymore
    {
        private string? currentMessage;
        private string totalWaterUsage = "0";
        public string? CurrentMessage
        {
            get => currentMessage;
            set
            {
                if (currentMessage != value)
                {
                    currentMessage = value;
                    OnPropertyChanged(nameof(CurrentMessage));
                    HideButton();
                }
            }
        }

        public bool DontHideMessage { get; set; }

        private void HideButton()
        {
            DontHideMessage = (IsProcessing) ? true : false;
        }

        private bool IsProcessing //To disable or enable el button
        {
            get
            {
                return CurrentMessage != "Now calculating water usage...";
            }
        }

        public string? TotalWaterUsage
        {
            get => totalWaterUsage;
            set
            {
                if (totalWaterUsage != value)
                {
                    totalWaterUsage = value;
                    OnPropertyChanged(nameof(TotalWaterUsage));
                }
            }
        }

        public string? WaterUsageActivity { get; set; } = null;

        public Messaging()
        {
            HideButton();
        }
    }
}
