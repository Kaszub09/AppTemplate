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
            _currentThemeName = null;
            AllThemes = new Dictionary<string, ResourceDictionary>();
            LoadAllThemes(@"appData\themes");
        }

        private static void LoadAllThemes(string folderPath) {
            LoadEmbeddedThemes();
            if (Directory.Exists(folderPath))
                LoadThemesInFolder(folderPath);

            Application.Current.Resources.MergedDictionaries.Add(AllThemes["Default"]);
            _currentThemeName = "Default";
        }

        private static void LoadEmbeddedThemes() {
            try {
                AllThemes.Add("Default", new ResourceDictionary() {
                    Source = new Uri("appData/themes/Default.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception) {//TO DO
            }

            try {
                AllThemes.Add("Dark", new ResourceDictionary() {
                    Source = new Uri("appData/themes/Dark.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception) {//TO DO
            }

            try {
                AllThemes.Add("DSIII", new ResourceDictionary() {
                    Source = new Uri("appData/themes/DSIII.xaml", UriKind.RelativeOrAbsolute)
                });
            } catch (Exception) { //TO DO
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


        public static void ChangeTheme(string themeName) {
            if (_currentThemeName!= themeName && AllThemes.ContainsKey(themeName)) {
                Application.Current.Resources.MergedDictionaries.Remove(AllThemes[_currentThemeName]);
                Application.Current.Resources.MergedDictionaries.Add(AllThemes[themeName]);
                _currentThemeName = themeName;
            }
        }
    }
}
