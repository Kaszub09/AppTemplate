using AppTemplate.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTemplate.Model;
using System.Windows.Forms;

namespace AppTemplate {
    public static class Tray {
        static private NotifyIcon _trayIcon;
        static private VMMain _MV;
        static public int DummyVariable;

        static Tray() {
            _MV = new VMMain();

            //Tray icon settings
            _trayIcon = new NotifyIcon();
            _trayIcon.Icon = Properties.Resources.trayIcon;
            _trayIcon.Text = Subtitles.GetText("app_name");
            _trayIcon.MouseClick += _trayIcon_MouseClick;
            _trayIcon.Visible = false;

            _trayIcon.ContextMenu = GetContextMenu();
            //Could use Popup as ContextMenu on _trayIcon element, but can't close it when cliked outside of popup
            //////Popup initialization
            ////var _popup = new Popup();
            ////_popup.Placement = PlacementMode.Mouse;
            ////_popup.StaysOpen = false;
            ////var menu = GetMenu();
            ////_popup.Child = menu;
            ////foreach (Control item in menu.Items) {
            ////    item.Click+=(sender,e) => _popup.IsOpen = false;
            ////}
            ////menu.MouseLeave += (sender, e) => _popup.IsOpen = false;

        }

        private static ContextMenu GetContextMenu() {
            var menu = new ContextMenu();
            _MV.VMText.PropertyChanged += (sender, e) => {
                foreach (MenuItem item in menu.MenuItems) {
                    if (item.Tag is string)
                        item.Text = _MV.VMText[(string)item.Tag];
                }
            };

            //Start process
            var menuItem1 = new MenuItem() { Tag = "button_actions_process_run", Text = Subtitles.GetText("button_actions_process_run") };
            menuItem1.Click += (sender, e) => Commands.AllCommands["RunProcess"].Execute(null);
            menu.MenuItems.Add(menuItem1);

            //Stop process
            var menuItem2 = new MenuItem() { Tag = "button_actions_process_stop", Text = Subtitles.GetText("button_actions_process_stop") };
            menuItem2.Click += (sender, e) => Commands.AllCommands["StopProcess"].Execute(null);
            menu.MenuItems.Add(menuItem2);

            ProgramModel.IsRunningChanged += (sender, e) => {
                if (ProgramModel.IsRunning == true) {
                    menuItem1.Enabled = false;
                    menuItem2.Enabled = true;
                } else {
                    menuItem1.Enabled = true;
                    menuItem2.Enabled = false;
                }
            };

            //Restore form tray
            var menuItem3 = new MenuItem() { Tag = "button_restore_from_tray", Text = Subtitles.GetText("button_restore_from_tray") };
            menuItem3.Click += (sender, e) => Commands.AllCommands["RestoreFromTray"].Execute(null);
            menu.MenuItems.Add(menuItem3);

            //Exit applciation
            var menuItem4 = new MenuItem() { Tag = "menu_button_exit", Text = Subtitles.GetText("menu_button_exit") };
            menuItem4.Click += (sender, e) => Commands.AllCommands["Exit"].Execute(null);
            menu.MenuItems.Add(menuItem4);

            return menu;
        }

        private static void _trayIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) {
                RestoreFromTray();
            } 
        }

        static public void MinimizeToTray() {
            _trayIcon.Visible = true;
            App.Current.MainWindow.Visibility = System.Windows.Visibility.Hidden;
            ShowNotification(2000, Subtitles.GetText("app_name"), Subtitles.GetText("app_minimised_ballontip"), ToolTipIcon.Info);
        }

        static public void RestoreFromTray() {
            App.Current.MainWindow.Visibility = System.Windows.Visibility.Visible;
            _trayIcon.Visible = false;
        }

        static public void ShowNotification(int timeout, string tipTitle, string tipText,ToolTipIcon tipIcon) {
            _trayIcon.ShowBalloonTip(timeout, tipTitle, tipText, tipIcon);
        }

        
    }
}
  
