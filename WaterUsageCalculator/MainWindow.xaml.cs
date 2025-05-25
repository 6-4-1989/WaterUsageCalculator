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

namespace FrontendStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InSession? inSession;

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
                    inSession = new InSession(filePath);
                    DataContext = inSession;
                    inSession.visualize.myPlot.Refresh();
                }
                else
                {
                    MessageBox.Show(".mp3 format, please");
                }
            }
            else
            {
                MessageBox.Show("please insert a file");
            }
        }
    }
}