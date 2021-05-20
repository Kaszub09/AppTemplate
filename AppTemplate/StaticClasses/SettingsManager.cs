using AppTemplate.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppTemplate.StaticClasses;
using System.IO;
using System.Xml;

namespace AppTemplate {
    public struct SettingsSet {
        public string CredentialsName;
        public string Username;
        public SecureString Password;
        public double IntervalSec;
        public int FontSize;
        public DateTime ChosedDate;
    }


    public static class SettingsManager {
        private static SettingsSet _settings;
        public static SettingsSet Settings {
            get {
                return _settings;
            }
            set {
                _settings = value;
                CredentialManager.WriteCreds(value.CredentialsName, value.Username, value.Password);
                SaveSettingsToFile();
                SettingsChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler SettingsChanged;
        private static string _settingsFilepath;

        static SettingsManager() {
            _settingsFilepath = @"appData\settings.xml";
            SetDefaultSettings();

            try {
                ReadSettingsFromFile();
            } catch (Exception e) {
                //TO DO LOGGER
            }
        }

        static public SettingsSet SettingsGetCopy() {
            var setts = new SettingsSet() {
                CredentialsName = Settings.CredentialsName,
                Username = Settings.Username,
                Password = Settings.Password,
                IntervalSec = Settings.IntervalSec,
                FontSize = Settings.FontSize,
                ChosedDate = Settings.ChosedDate
            };
            return setts;
        }

        static public void SetDefaultSettings() {
            var sets = new SettingsSet() {
                CredentialsName = "AppTemplateCredentials",
                IntervalSec = 60,
                FontSize = 12,
                ChosedDate = DateTime.Today
            };
            var cred = CredentialManager.ReadCredsSecureString(sets.CredentialsName);
            sets.Password = cred.password;
            sets.Username = cred.username;
            _settings = sets;
        }

        static private void SaveSettingsToFile() {
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilepath));
            if (File.Exists(_settingsFilepath)) {
                File.Delete(_settingsFilepath);
            }

            //Create document with setings node
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlNode settingsNode = doc.CreateElement("Settings");
            doc.AppendChild(settingsNode);

            //Creds name
            var singleSetting = doc.CreateElement("Setting");
            singleSetting.SetAttribute("id", "cred_name");
            singleSetting.InnerText = Settings.CredentialsName;
            settingsNode.AppendChild(singleSetting);

            //Date
            singleSetting = doc.CreateElement("Setting");
            singleSetting.SetAttribute("id", "date_utc");
            singleSetting.InnerText = Settings.ChosedDate.ToString("O");
            settingsNode.AppendChild(singleSetting);

            //Interval
            singleSetting = doc.CreateElement("Setting");
            singleSetting.SetAttribute("id", "interval_sec");
            singleSetting.InnerText = Settings.IntervalSec.ToString();
            settingsNode.AppendChild(singleSetting);

            //Font size
            singleSetting = doc.CreateElement("Setting");
            singleSetting.SetAttribute("id", "font_size");
            singleSetting.InnerText = Settings.FontSize.ToString();
            settingsNode.AppendChild(singleSetting);

            doc.Save(_settingsFilepath);
        }



        static public void ReadSettingsFromFile() {
            var setts = SettingsGetCopy();

            if (File.Exists(_settingsFilepath)) {
                XmlDocument doc = new XmlDocument();
                doc.Load(_settingsFilepath);

                foreach (XmlElement node in doc.GetElementsByTagName("Setting")) {
                    switch (node.GetAttribute("id")) {
                        case "cred_name":
                            setts.CredentialsName = node.InnerText;
                            break;
                        case "date_utc":
                            setts.ChosedDate = DateTime.Parse(node.InnerText);
                            break;
                        case "interval_sec":
                            setts.IntervalSec = double.Parse(node.InnerText);
                            break;
                        case "font_size":
                            setts.FontSize = int.Parse(node.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }

            var cred = CredentialManager.ReadCredsSecureString(setts.CredentialsName);
            setts.Password = cred.password;
            setts.Username = cred.username;

            Settings = setts;
        }
    }
}
