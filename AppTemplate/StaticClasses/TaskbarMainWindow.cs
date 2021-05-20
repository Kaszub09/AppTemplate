using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace AppTemplate {
    static public class TaskbarMainWindow {
        static private TaskbarItemInfo _taskbar;

        static TaskbarMainWindow() {
            if(Application.Current.MainWindow.TaskbarItemInfo == null) 
                Application.Current.MainWindow.TaskbarItemInfo = new TaskbarItemInfo();
            _taskbar = Application.Current.MainWindow.TaskbarItemInfo;
        }

        static public void ChangeState(double? value = null, TaskbarItemProgressState? state=null, string description=null) {
            _taskbar.ProgressState = state==null? _taskbar.ProgressState:state.Value;
            _taskbar.ProgressValue = value == null ? _taskbar.ProgressValue : value.Value;
            _taskbar.Description = description == null ? _taskbar.Description : description;
        }

    }
}
