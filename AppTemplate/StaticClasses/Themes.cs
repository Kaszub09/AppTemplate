using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace AppTemplate {
    static class Themes {
        public static Dictionary<string, ResourceDictionary> AllThemes { get; private set; }
        private static string _currentThemeName;

        static Themes() {
            AllThemes = new Dictionary<string, ResourceDictionary>();
            LoadAllThemes(SettingsManager.Settings.DataFolder);

            Application.Current.Resources.MergedDictionaries.Add(AllThemes["Default"]);
            _currentThemeName = "Default";
            ChangeTheme(SettingsManager.Settings.ThemeID);           
        }

        public static void ChangeTheme(string themeName) {
            if (_currentThemeName != themeName && AllThemes.ContainsKey(themeName)) {
                Application.Current.Resources.MergedDictionaries.Remove(AllThemes[_currentThemeName]);
                Application.Current.Resources.MergedDictionaries.Add(AllThemes[themeName]);
                _currentThemeName = themeName;
                SettingsManager.Settings = new SettingsSet(false) { ThemeID = themeName };
            }
        }

        #region LoadThemes
        private static void LoadAllThemes(string folderPath) {
            LoadEmbeddedThemes(folderPath);
            if (Directory.Exists(folderPath))
                LoadThemesInFolder(folderPath);
        }

        private static void LoadEmbeddedThemes(string folderPath) {
            try {
                AllThemes.Add("Default", new ResourceDictionary() {
                    Source = new Uri(folderPath +"/themes/Default.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception e) {//TO DO
                AllThemes.Add("Default", new ResourceDictionary());
            }

            try {
                AllThemes.Add("Dark", new ResourceDictionary() {
                    Source = new Uri(folderPath + "/themes/Dark.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception e) {//TO DO
            }

            try {
                AllThemes.Add("DSIII", new ResourceDictionary() {
                    Source = new Uri(folderPath + "/themes/DSIII.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception e) { //TO DO
            }
        }

        private static void LoadThemesInFolder(string folderPath) {
            var regEx = new Regex(@"\\([^\\]+?).xaml");
         
            foreach (var file in Directory.GetFiles(folderPath, "*.xaml")) {
                try {
                    var res = new ResourceDictionary();
                    res.Source = new Uri("pack://siteoforigin:,,,/" + file.Replace(@"\\", "/"), UriKind.RelativeOrAbsolute);

                    AllThemes[regEx.Match(file).Groups[1].Value] = res;
                } catch (Exception e) {
                    //TO DO LOGGER
                }
            }
        }
        #endregion LoadThemes


    }
}
