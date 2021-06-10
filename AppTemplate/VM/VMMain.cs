using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppTemplate.VM {

    public class VMMain : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        public VMText VMText { get; private set; }
        public VMCommands VMCommands { get; private set; }
        public Page CurrentPage { get; private set; }

        private string _leftStatusBar;
        public string LeftStatusBar{
            get {
                return _leftStatusBar;
            }
            set {
                _leftStatusBar = value;
                OnPropertyChanged();
            }
        }

        private string _middleStatusBar;
        public string MiddleStatusBar {
            get {
                return _middleStatusBar;
            }
            set {
                _middleStatusBar = value;
                OnPropertyChanged();
            }
        }

        private int _progressBarValue;
        public int ProgressBarValue {
            get {
                return _progressBarValue;
            }
            set {
                _progressBarValue = value;
                OnPropertyChanged();
            }
        }

        private int _fontSize;
        public int FontSize {
            get {
                return _fontSize;
            }
            set {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        public VMMain() {
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

        ~VMMain() {
            PagesManager.PageChanged -= PagesManager_PageChanged;
            SettingsManager.SettingsChanged -= Settings_SettingsChanged;
        }
    }

}
