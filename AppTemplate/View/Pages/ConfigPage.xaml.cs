using AppTemplate.VM;
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


namespace AppTemplate.View.Pages {
    /// <summary>
    /// Logika interakcji dla klasy ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page, INavigablePage {
        public ConfigPage() {
            InitializeComponent();
            SaveSettingsButton.Click += SaveSettingsButton_Click;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e) {
            CredentialManager.WriteCreds(SettingsManager.Settings.CredentialsName, Username.Text, Password.Password);

            var setts = new SettingsSet(false) { IntervalSec = double.Parse(IntervalSec.Text) };
            if (ChosedDate.SelectedDate.HasValue)
                setts.ChosedDate = ChosedDate.SelectedDate.Value;

            setts.ShowNotifications = ShowNotifications.IsChecked.Value;

            Commands.AllCommands["SaveSettings"].Execute(setts);
        }

        public void OnNavigatedFrom() {
            var credentials = CredentialManager.ReadCreds(SettingsManager.Settings.CredentialsName);

            //Check if settings changed
            bool settingsChanged = Username.Text != credentials.UserName ||
                Password.Password != credentials.Password ||
                ChosedDate.SelectedDate != SettingsManager.Settings.ChosedDate ||
                double.Parse(IntervalSec.Text) != SettingsManager.Settings.IntervalSec ||
                ShowNotifications.IsChecked.Value != SettingsManager.Settings.ShowNotifications;

            if (settingsChanged) {
                //Ask, if user wants to save them
                var result = MessageBox.Show(Subtitles.GetText("msgbox_save_settings_text"), Subtitles.GetText("msgbox_save_settings_caption"), 
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                if (result == MessageBoxResult.Yes)
                    SaveSettingsButton_Click(SaveSettingsButton,new RoutedEventArgs());
            }
        }

        public void OnNavigatedTo(object parameter) {
            //Read parameters from settings
            var credentials = CredentialManager.ReadCreds(SettingsManager.Settings.CredentialsName);
            Username.Text = credentials.UserName;
            Password.Password = credentials.Password;

            ChosedDate.SelectedDate = SettingsManager.Settings.ChosedDate;
            IntervalSec.Text = SettingsManager.Settings.IntervalSec.ToString();
            ShowNotifications.IsChecked = SettingsManager.Settings.ShowNotifications;
        }

        private void Interval_LostFocus(object sender, RoutedEventArgs e) {
            double x;
            if (double.TryParse(IntervalSec.Text, out x) == false) {
                MessageBox.Show(Subtitles.GetText("msgbox_wrong_interval_text"), Subtitles.GetText("msgbox_wrong_data_generic_caption"), 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                IntervalSec.Text = SettingsManager.Settings.IntervalSec.ToString();
            }
        }

    }
}
