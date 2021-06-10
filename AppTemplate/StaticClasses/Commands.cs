using AppTemplate.Model;
using AppTemplate.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTemplate {
    static class Commands {
        public static event EventHandler CommandsChanged;
        //Private properties
        public static Dictionary<string, ICommand> AllCommands { get; private set; }
        public static Frame Frame;

        static Commands() {

            AllCommands = new Dictionary<string, ICommand>();

            AllCommands["ChangeLanguage"] = new Command((Object obj) => {
                Subtitles.ChangeLanguage((string)obj);
            });

            AllCommands["ChangeTheme"] = new Command((Object obj) => {
                Themes.ChangeTheme((string)obj);
            });

            AllCommands["Exit"] = new Command((Object obj) => {
                App.Current.Shutdown();
            });

            AllCommands["ChangePage"] = new Command((Object obj) => {
                if (obj is Type) {
                    PagesManager.Navigate((Type)obj);
                } else if (obj is string) {
                    PagesManager.Navigate((string)obj);
                }
            });

            AllCommands["GoBack"] = new Command((Object obj) => { PagesManager.GoBack(); },
                (object obj) => { return PagesManager.CanGoBack(); });

            AllCommands["GoForward"] = new Command((Object obj) => { PagesManager.GoForward(); },
                (object obj) => { return PagesManager.CanGoForward(); });

            AllCommands["MinimizeToTray"] = new Command((Object obj) => Tray.MinimizeToTray());

            AllCommands["RestoreFromTray"] = new Command((Object obj) => Tray.RestoreFromTray());

            AllCommands["SaveSettings"] = new Command((Object obj) => {
                if (obj is SettingsSet) {
                    SettingsManager.Settings = (SettingsSet)obj;
                }
            });

            AllCommands["ChangeFontSize"] = new Command((Object obj) => {
                if (obj is string) {
                    int size;
                    if (int.TryParse((string)obj, out size)) {
                        var currentSettings = new SettingsSet(false) { FontSize = size };
                        SettingsManager.Settings = currentSettings;
                    }
                }
            });

            AllCommands["RunProcess"] = new Command((object obj) => ProgramModel.Start(),(object obj)=>!ProgramModel.IsRunning);

            AllCommands["StopProcess"] = new Command((object obj) => ProgramModel.Stop(), (object obj) => ProgramModel.IsRunning);
        }
    }
}
