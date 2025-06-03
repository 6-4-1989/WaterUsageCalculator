using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterUsageBoilerplate.Models;
using WaterUsageBoilerplate.Viewers;
using ScottPlot;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;
using System.Globalization;
using MathNet.Numerics.Distributions;

namespace FrontendStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InSession? inSession;
        private double waterUsage = 0;
        private string selected = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Handle files
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;

                if (filePath.Substring(filePath.Length - 3, 3) == "wav")
                {
                    MessageBox.Show(filePath);
                    inSession = new InSession(filePath, selected);
                    DataContext = inSession;
                    waterUsage += double.Parse(inSession.messaging.TotalWaterUsage, CultureInfo.InvariantCulture);
                    CurrentWaterUsage.Content = $"Liters: {waterUsage.ToString()}";
                    inSession.visualize.myPlot.Refresh();
                }
                else
                {
                    MessageBox.Show(".wav format, please");
                }
            }
            else
            {
                MessageBox.Show("please insert a file");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ComboBoxItem)(sender as ComboBox).SelectedItem != null)
            {
                ComboBoxItem comboBoxItem = (ComboBoxItem)(sender as ComboBox).SelectedItem;
                selected = comboBoxItem.Content.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentWaterUsage.Content = "Liters: 0";
            waterUsage = 0;
        }
    }
}