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
using System.Net;
using System.IO;

namespace Sabre
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : MetroWindow
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        private void updateWindow_Initialized(object sender, EventArgs e)
        {
            string patchNotesOnlineLink = "https://drive.google.com/uc?export=download&id=0Bz9aB-8O_UqfQkNaRlJfbjhMQnM";
            string patchNotesString;
            WebClient wc = new WebClient();
            patchNotesString = wc.DownloadString(patchNotesOnlineLink);
            patchNotesList.Items.Add(patchNotesString);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string updaterDownloadLink = "https://drive.google.com/uc?export=download&id=0Bz9aB-8O_UqfUF9ldk54Qy1sTm8";
            WebClient wc = new WebClient();
            wc.DownloadFile(updaterDownloadLink, Environment.CurrentDirectory + @"\sabreupdater.exe");
            MessageBox.Show("Download of the updater is complete. Sabre exits now in order to run the upgrade.");
            System.Diagnostics.Process.Start("sabreupdater.exe");
            Environment.Exit(1);
        }
    }
}
