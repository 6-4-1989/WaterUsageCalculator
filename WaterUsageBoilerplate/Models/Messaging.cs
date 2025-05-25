using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterUsageBoilerplate.Viewers;

namespace WaterUsageBoilerplate.Models
{
    public class Messaging : PropertyChangedClass //Handles some random crud I swear I can't anymore
    {
        private string? currentMessage;
        public string? CurrentMessage
        {
            get
            {
                return currentMessage;
            }
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

        public void HideButton()
        {
            DontHideMessage = (IsProcessing) ? true : false;
        }

        public bool IsProcessing //To disable or enable el button
        {
            get
            {
                return CurrentMessage != "Now calculating water usage...";
            }
        }
    }
}
