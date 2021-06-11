using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTemplate.VM;
using AppTemplate;
using ProgramTemplate;

namespace AppTemplate.Model {
    
    public enum CurrentState {
        Running,
        NotRunning
    }


    public static class ProgramModel {
        private static bool _isRunning;
        public static bool IsRunning {
            get {
                return _isRunning;
            }
            private set {
                _isRunning = value;
                IsRunningChanged?.Invoke(null, value);
            }
        }
        public static event EventHandler<bool> IsRunningChanged;

        private static Program _program;
        private static Progress<ProgressInfo> _processProgress;
        private static Progress<DateTime?> _nextRun;

        static ProgramModel() {
            Tray.DummyVariable = 0;
            _processProgress = new Progress<ProgressInfo>();
            _nextRun = new Progress<DateTime?>();

            _processProgress.ProgressChanged += _processProgress_ProgressChanged;
            _nextRun.ProgressChanged += _nextRun_ProgressChanged;

            _program = GetProgramBasedOnSettings();
        }

        private static void _nextRun_ProgressChanged(object sender, DateTime? e) {
            if (e.HasValue == true) {
                ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar =
                    string.Format(Subtitles.GetText("left_status_bar_next_run_time"), e.Value.Subtract(DateTime.Now));
            } else {
                ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar = Subtitles.GetText("left_status_bar_next_run_never");
            }
        }

        private static void _processProgress_ProgressChanged(object sender, ProgressInfo e) {
            var MV = ((VMMain)App.Current.MainWindow.DataContext);
            if (e.Status == Status.Running) {
                MV.MiddleStatusBar = string.Format(Subtitles.GetText("middle_status_bar_proces_active_info"), e.ProcessedItems, e.AllItemsNumber);

                if (e.AllItemsNumber > 0) {
                    MV.ProgressBarValue = e.ProcessedItems * 100 / e.AllItemsNumber;
                    TaskbarMainWindow.ChangeState(((float)e.ProcessedItems) / e.AllItemsNumber, System.Windows.Shell.TaskbarItemProgressState.Normal);
                }
            } else {
                MV.MiddleStatusBar = string.Format(Subtitles.GetText("middle_status_bar_proces_completed_info"), DateTime.Now, e.ProcessedItems, e.AllItemsNumber);
                MV.ProgressBarValue = e.ProcessedItems * 100 / e.AllItemsNumber;
                TaskbarMainWindow.ChangeState(state: System.Windows.Shell.TaskbarItemProgressState.Paused);
                Tray.ShowNotification(1000, "", string.Format(Subtitles.GetText("middle_status_bar_proces_completed_info"), DateTime.Now, e.ProcessedItems, e.AllItemsNumber), System.Windows.Forms.ToolTipIcon.Info);
            }
        }

        static public void Start() {
            if (_program.IsRunning == false) {
                _program = GetProgramBasedOnSettings();
                _program.Start();

                if (_program.IsRunning == true) {
                    ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar = Subtitles.GetText("left_status_bar_program_started");
                    TaskbarMainWindow.ChangeState(state: System.Windows.Shell.TaskbarItemProgressState.Paused);
                    IsRunning = true;
                } else {
                    ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar = Subtitles.GetText("left_status_bar_program_start_error");
                    IsRunning = false;
                }
            }
        }

        static public void Stop() {
            if (_program.IsRunning == true) {
                _program.Stop();

                if (_program.IsRunning == false) {
                    ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar = Subtitles.GetText("left_status_bar_program_stopped");
                    TaskbarMainWindow.ChangeState(state: System.Windows.Shell.TaskbarItemProgressState.None);
                    IsRunning = false;
                } else {
                    ((VMMain)App.Current.MainWindow.DataContext).LeftStatusBar = Subtitles.GetText("left_status_bar_program_stop_error");
                    IsRunning = true;
                }
            }
        }

        private static Program GetProgramBasedOnSettings() {
            var cred = CredentialManager.ReadCreds(SettingsManager.Settings.CredentialsName);
            var parameters = new ProcessParameters() { 
                Date = SettingsManager.Settings.ChosedDate, 
                Username = cred.UserName, 
                Password = cred.SecurePassword };
            var program = new Program(parameters, SettingsManager.Settings.IntervalSec * 1000, _nextRun, _processProgress);
            return program;
        }

    }
}
