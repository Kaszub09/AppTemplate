using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppTemplate.VM {

    public class MainViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        public VMText VMText { get; private set; }
        public VMCommands VMCommands { get; private set; }
        public Page CurrentPage { get; private set; }
        public string LeftStatusBar{
            get {
                return _leftStatusBar;
            }
            set {
                _leftStatusBar = value;
                OnPropertyChanged();
            }
        }
        public string MiddleStatusBar {
            get {
                return _middleStatusBar;
            }
            set {
                _middleStatusBar = value;
                OnPropertyChanged();
            }
        }
        public int ProgressBarValue {
            get {
                return _progressBarValue;
            }
            set {
                _progressBarValue = value;
                OnPropertyChanged();
            }
        }
        public int FontSize {
            get {
                return _fontSize;
            }
            set {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        private string _middleStatusBar;
        private int _progressBarValue;
        private string _leftStatusBar;
        private int _fontSize;

        public MainViewModel() {
            VMText = new VMText();
            VMCommands = new VMCommands();

            ProgressBarValue = 0;
            MiddleStatusBar = "";
            LeftStatusBar = "";
            FontSize = SettingsManager.Settings.FontSize;

            PagesManager.PageChanged += PagesManager_PageChanged;
            SettingsManager.SettingsChanged += Settings_SettingsChanged;
        }

        private void Settings_SettingsChanged(object sender, EventArgs e) {
            FontSize = SettingsManager.Settings.FontSize;
        }

        private void PagesManager_PageChanged(object sender, EventArgs e) {
            CurrentPage = (Page)PagesManager.CurrentPage;
            OnPropertyChanged("CurrentPage");
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        ~MainViewModel() {
            PagesManager.PageChanged -= PagesManager_PageChanged;
            SettingsManager.SettingsChanged -= Settings_SettingsChanged;
        }
    }

}
