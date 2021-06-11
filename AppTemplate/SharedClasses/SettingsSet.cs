using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTemplate {
    public class SettingsSet {
        public string CredentialsName = "AppTemplateCredentials";
        public string DataFolder = @"data";
        public string ThemeID = "Default";
        public string LanguageID = "pol";
        public double IntervalSec = 60;
        public int FontSize = 12;
        public DateTime ChosedDate = DateTime.Today;
        public bool ShowNotifications=true;

        public SettingsSet():this(false) {
        }

        public SettingsSet(bool defaultSettings) {
            if (defaultSettings == false) {
                CredentialsName = SettingsManager.Settings.CredentialsName;
                DataFolder = SettingsManager.Settings.DataFolder;
                ThemeID = SettingsManager.Settings.ThemeID;
                LanguageID = SettingsManager.Settings.LanguageID;
                IntervalSec = SettingsManager.Settings.IntervalSec;
                FontSize = SettingsManager.Settings.FontSize;
                ChosedDate = SettingsManager.Settings.ChosedDate;
                ShowNotifications = SettingsManager.Settings.ShowNotifications;
            }
        }
    }
    
}
