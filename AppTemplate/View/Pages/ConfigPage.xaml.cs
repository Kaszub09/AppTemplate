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
            if (!(this.DataContext is MainViewModel))
                this.DataContext = new MainViewModel();
        }

        public void OnNavigatedFrom() {
            var mainVM = (MainViewModel)this.DataContext;

            //Check if settings changes
            if (Username.Text != SettingsManager.Settings.Username || ChosedDate.SelectedDate != SettingsManager.Settings.ChosedDate || 
                double.Parse(IntervalSec.Text) != SettingsManager.Settings.IntervalSec ||
                Password.Password != new System.Net.NetworkCredential(string.Empty, SettingsManager.Settings.Password).Password) {

                    //Ask, if user wants to save them
                    var result = MessageBox.Show(mainVM.VMText["msgbox_save_settings_text"], mainVM.VMText["msgbox_save_settings_caption"], MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    if (result == MessageBoxResult.Yes)
                        SaveSettingsButton.Command.Execute(SaveSettingsButton.CommandParameter);
            }
        }

        public void OnNavigatedTo(object parameter) {
            //Read parameters from settings
            Password.Password = new System.Net.NetworkCredential(string.Empty, SettingsManager.Settings.Password).Password;
            Username.Text = SettingsManager.Settings.Username;
            ChosedDate.SelectedDate = SettingsManager.Settings.ChosedDate;
            IntervalSec.Text = SettingsManager.Settings.IntervalSec.ToString();
        }

        private void Interval_LostFocus(object sender, RoutedEventArgs e) {
            var mainVM = (MainViewModel)this.DataContext;
            double x;
            if (double.TryParse(IntervalSec.Text, out x) == false) {
                MessageBox.Show(mainVM.VMText["msgbox_wrong_interval_text"], mainVM.VMText["msgbox_wrong_data_generic_caption"], MessageBoxButton.OK, MessageBoxImage.Warning);
                IntervalSec.Text = SettingsManager.Settings.IntervalSec.ToString();
            }
        }
    }
}
