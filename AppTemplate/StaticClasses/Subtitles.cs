﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppTemplate {
    static public class Subtitles {
        public static event EventHandler LanguageChanged;
        public static Dictionary<string, string> Text { get; private set; }
        public static Dictionary<string, Dictionary<string, string>> AllLanguages { get; private set; }


        static Subtitles() {
            AllLanguages = new Dictionary<string, Dictionary<string, string>>();
            LoadAllLanguages(@"appData\lang");
        }

        public static string GetText(string key) {
            if (Text.ContainsKey(key)) {
                return Text[key];
            } else {
                return "[" + key + "]";
            }
        }

        private static void LoadAllLanguages(string folderPath) {
            LoadEmbeddedLanguages();
            if (Directory.Exists(folderPath)) {
                LoadXMLLanguages(folderPath);
                LoadTXTLanguages(folderPath);
            }

            ChangeLanguage("pol");
        }

        private static void LoadEmbeddedLanguages() {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Properties.Resources.EmbeddedLanguages);

            var langNodes = doc.GetElementsByTagName("Language");

            foreach (XmlElement langNode in langNodes) {
                var lang = new Dictionary<string, string>();

                foreach (var node in langNode.ChildNodes) {
                    if (node is XmlElement) {
                        lang[((XmlElement)node).GetAttribute("id")] = ((XmlElement)node).InnerText.Trim();
                    }
                }

                lang["language_id"] = langNode.GetAttribute("id");
                lang["language_display_name"] = langNode.GetAttribute("name");

                AllLanguages[lang["language_id"]] = lang;
            } 
        }


        private static void LoadXMLLanguages(string folderPath) {
            foreach (var file in Directory.GetFiles(folderPath, "*.xml")) {
                try {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);

                    var langNodes = doc.GetElementsByTagName("Language");

                    foreach (XmlElement langNode in langNodes) {
                        var lang = new Dictionary<string, string>();

                        foreach (var node in langNode.ChildNodes) {
                            if (node is XmlElement) {
                                lang[((XmlElement)node).GetAttribute("id")] = ((XmlElement)node).InnerText;
                            }
                        }
                        lang["language_id"] = langNode.GetAttribute("id");
                        lang["language_display_name"] = langNode.GetAttribute("name");

                        AllLanguages[lang["language_id"]] = lang;
                    }
                } catch (Exception e) {
                    //TO DO LOGGER
                }
            }
        }

        private static void LoadTXTLanguages(string folderPath) {
            foreach (var file in Directory.GetFiles(folderPath, "*.txt")) {
                try {
                    var lang = new Dictionary<string, string>();

                    var lines = File.ReadAllLines(file);

                    foreach (var line in lines) {
                        if (line.Trim().Length > 0 && !line.StartsWith("#") && line.Contains("==")) {
                            var sLine = line.Split("==".ToArray());
                            lang[sLine[0].Trim()] = System.Text.RegularExpressions.Regex.Unescape(sLine[1]);
                        }
                    }

                    if (lang.ContainsKey("language_id") == false)
                        lang["language_id"] = "Unspecified";
                    if (lang.ContainsKey("language_display_name") == false)
                        lang["language_display_name"] = "Unspecified";

                    AllLanguages[lang["language_id"]] = lang;
                } catch (Exception e) {
                    //TO DO LOGGER
                }
            }
        }

        public static void ChangeLanguage(string language) {
            if (AllLanguages.ContainsKey(language)) {
                Text = AllLanguages[language];
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }

    }





}
