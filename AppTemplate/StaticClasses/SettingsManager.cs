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

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AppTemplate {


    public static class SettingsManager {
        private static SettingsSet _settings;
        public static SettingsSet Settings {
            get {
                return _settings;
            }
            set {
                _settings = value;
                SaveSettingsToFile();
                SettingsChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler SettingsChanged;
        private static string _settingsFilepath;

        static SettingsManager() {
            SetDefaultSettings();
            _settingsFilepath = Settings.DataFolder + @"\settings.xml";

            try {
                ReadSettingsFromFile();
            } catch (Exception e) {
                //TO DO LOGGER
            }
        }

        public static void SetDefaultSettings() {
            _settings = new SettingsSet(true) ;
        }

        #region Serializer
        private static void SaveSettingsToFile() {
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilepath));
            if (File.Exists(_settingsFilepath)) {
                File.Delete(_settingsFilepath);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SettingsSet));
            using FileStream file = File.Create(_settingsFilepath);

            serializer.Serialize(file, Settings);
            file.Close();
        }

        static public void ReadSettingsFromFile() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingsSet));
                using FileStream file = File.OpenRead(_settingsFilepath);

                Settings = (SettingsSet)serializer.Deserialize(file);
                file.Close();
            } catch(Exception e) {

            }
            
        }
        #endregion Serializer
    }
}
