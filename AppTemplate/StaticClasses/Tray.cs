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
        static private MainViewModel _MV;
        static public int DummyVariable;

        static Tray() {
            _MV = new MainViewModel();

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

            //Menu item 1...
            var menuItem1 = new MenuItem() { Tag = "button_actions_process_run", Text = _MV.VMText["button_actions_process_run"] };
            menuItem1.Click += MenuItem_ClickStart;
            menu.MenuItems.Add(menuItem1);

            ProgramModel.StartStopStateChange += (sender, e) => {
                if (e == CurrentState.Running) {
                    menuItem1.Tag = "button_actions_process_stop";
                    menuItem1.Click -= MenuItem_ClickStart;
                    menuItem1.Click += MenuItem_ClickStop;
                } else {
                    menuItem1.Tag = "button_actions_process_run";
                    menuItem1.Click -= MenuItem_ClickStop;
                    menuItem1.Click += MenuItem_ClickStart;
                }
                menuItem1.Text = _MV.VMText[(string)menuItem1.Tag];
            };

            //Menu item 2...
            var menuItem2 = new MenuItem() { Tag = "button_restore_from_tray", Text = _MV.VMText["button_restore_from_tray"] };
            menuItem2.Click += (sender, e) => _MV.VMCommands["RestoreFromTray"].Execute(null);
            menu.MenuItems.Add(menuItem2);

            //Menu item 3 ...
            var menuItem3 = new MenuItem() { Tag = "menu_button_exit", Text = _MV.VMText["menu_button_exit"] };
            menuItem3.Click += (sender, e) => _MV.VMCommands["Exit"].Execute(null);
            menu.MenuItems.Add(menuItem3);

            return menu;
        }

        private static void MenuItem_ClickStart(object sender, EventArgs e) {
            _MV.VMCommands["RunProcess"].Execute(null);
        }

        private static void MenuItem_ClickStop(object sender, EventArgs e) {
            _MV.VMCommands["StopProcess"].Execute(null);
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
  
