using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AppTemplate.Model;
using AppTemplate.View.Pages;

namespace AppTemplate.VM {
    public class VMCommands : INotifyPropertyChanged {
        private string _startStopButtonID;
        public ICommand StartStopButton {
            get {
                return this[_startStopButtonID];
            }
            private set {      
            }
        }
        //Public properties
        [IndexerName("Item")]
        public ICommand this[string key] {
            get {
                return _commands[key];
            }
        }

        //Private properties
        private Dictionary<string, ICommand> _commands;
        public Frame Frame;

        //Public properties
        public event PropertyChangedEventHandler PropertyChanged;

        public VMCommands() {
            _startStopButtonID = "RunProcess";
            ProgramModel.StartStopStateChange += ProgramModel_StartStopStateChange;

            _commands = new Dictionary<string, ICommand>();

            _commands["ChangeLanguage"] = new Command((Object obj) => {
                Subtitles.ChangeLanguage((string)obj);
            });

            _commands["ChangeTheme"] = new Command((Object obj) => {
                Themes.ChangeTheme((string)obj);
            });

            _commands["Exit"] = new Command((Object obj) => {
                App.Current.Shutdown();
            });

            _commands["ChangePage"] = new Command((Object obj) => {
                if (obj is Type) {
                    PagesManager.Navigate((Type)obj);
                } else if (obj is string) {
                    PagesManager.Navigate((string)obj);
                }
            });

            _commands["GoBack"] = new Command((Object obj) => { PagesManager.GoBack(); },
                (object obj) => { return PagesManager.CanGoBack(); });

            _commands["GoForward"] = new Command((Object obj) => { PagesManager.GoForward(); },
                (object obj) => { return PagesManager.CanGoForward(); });

            _commands["MinimizeToTray"] = new Command((Object obj) => Tray.MinimizeToTray());

            _commands["RestoreFromTray"] = new Command((Object obj) => Tray.RestoreFromTray());

            _commands["SaveSettings"] = new Command((Object obj) => {
                if (obj is ConfigPage) {
                    var currentSettings = SettingsManager.SettingsGetCopy();
                    currentSettings.Username = ((ConfigPage)obj).Username.Text;
                    currentSettings.Password = ((ConfigPage)obj).Password.SecurePassword;
                    if(((ConfigPage)obj).ChosedDate.SelectedDate.HasValue ==false)
                        currentSettings.ChosedDate = ((ConfigPage)obj).ChosedDate.SelectedDate.Value;
                    currentSettings.IntervalSec = int.Parse(((ConfigPage)obj).IntervalSec.Text);
                    SettingsManager.Settings = currentSettings;
                }
            });

            _commands["ChangeFontSize"] = new Command((Object obj) => {
                if (obj is string) {
                    int size;
                    if (int.TryParse((string)obj, out size)) {
                        var currentSettings = SettingsManager.SettingsGetCopy();
                        currentSettings.FontSize = size;
                        SettingsManager.Settings = currentSettings;
                    }
                }
            });

            _commands["RunProcess"] = new Command((Object obj) => ProgramModel.Start());

            _commands["StopProcess"] = new Command((Object obj) => ProgramModel.Stop());

            OnPropertyChanged("StartStopButton");
        }

        private void ProgramModel_StartStopStateChange(object sender, CurrentState e) {
            if (e == CurrentState.Running) {
                _startStopButtonID = "StopProcess";
            } else {
                _startStopButtonID = "RunProcess";
            }
            OnPropertyChanged("StartStopButton");
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Destructor
        ~VMCommands() {

        }
    }
}
