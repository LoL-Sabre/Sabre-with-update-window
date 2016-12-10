using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Forms;
using System.Net;

namespace Sabre
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Window updateWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openSettingsWindow(object sender, RoutedEventArgs e)
        {
            if(flyoutSettings.Visibility == Visibility.Hidden)
            {
                flyoutSettings.Visibility = Visibility.Visible;
            }
            else
            {
                flyoutSettings.Visibility = Visibility.Hidden;
            }
        }

        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            checkForUpdate();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            updateWindow = new UpdateWindow();

            updateWindow.ShowDialog();
        }

        public void checkForUpdate()
        {
            //Update window
            updateWindow = new UpdateWindow();
            //Link of the uploaded version text.
            string latestVersionLink = "https://drive.google.com/uc?export=download&id=0Bz9aB-8O_UqfY1dqdTZjMUxMVFk";
            //A local string will be equal to the string downloaded by the webclient.
            string latestVersionOnline;
            WebClient wc = new WebClient();
            latestVersionOnline = wc.DownloadString(latestVersionLink);
            //A local string will be equal to the application's build number which can be changed as the program goes through further updates by right clicking in the Solution explorer Sabre->Properties->Application->Assembly information->Change Assembly and File versions.
            string currentVersion;
            currentVersion = System.Windows.Forms.Application.ProductVersion;

            updateButton.Content = "Upgrade to " + latestVersionOnline;
            //If the downloaded string's content is not equal to the current version's, the update window will pop up.
            if (latestVersionOnline != currentVersion)
            {
                updateButton.IsEnabled = true;
                updateButton.Visibility = Visibility.Visible;
                updateWindow.ShowDialog();
            }
        }
    }
}
