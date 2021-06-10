using AppTemplate.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppTemplate.View.Pages;
using System.ComponentModel;
using AppTemplate.View;
using System.Windows.Controls.Primitives;

namespace AppTemplate {
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            if (!(this.DataContext is VMMain))
                this.DataContext = new VMMain();

            UpdateLanguageMenuButtons();
            UpdateThemeMenuButtons();

            ((VMMain)this.DataContext).VMCommands["ChangePage"].Execute(typeof(ActionsPage));
        }

        private void UpdateLanguageMenuButtons() {
            var commands = ((VMMain)this.DataContext).VMCommands;
            foreach (var lang in Subtitles.AllLanguages.Values) {
                var langMenuItem = new MenuItem() {
                    Header = lang["language_display_name"],
                    Command = commands["ChangeLanguage"],
                    CommandParameter = lang["language_id"]
                };
                ////Binding SAMPLE
                //Binding myBinding = new Binding("VMText[language_display_name]");
                //myBinding.Source = ((MainViewModel)this.DataContext);
                //langMenuItem.SetBinding(MenuItem.HeaderProperty , myBinding);
                
                ////Radio button behaviour
                //langMenuItem.Click += (sender, e) => {
                //    foreach (MenuItem item in _topMenuLanguage.Items) {
                //        item.IsChecked = false;
                //    }
                //    ((MenuItem)sender).IsChecked = true;
                //};
                _topMenuLanguage.Items.Add(langMenuItem);
            }
        }

        private void UpdateThemeMenuButtons() {
            var commands = ((VMMain)this.DataContext).VMCommands;
            foreach (var themeName in Themes.AllThemes.Keys) {
                var themeMenuItem = new MenuItem() {
                    Command = commands["ChangeTheme"],
                    CommandParameter = themeName
                };

                Binding binding= new Binding("VMText[theme_"+ themeName +"]");
                binding.Source = (VMMain)this.DataContext;
                themeMenuItem.SetBinding(MenuItem.HeaderProperty, binding);

                _topMenuTheme.Items.Add(themeMenuItem);
            }
        }

        
    }
}
