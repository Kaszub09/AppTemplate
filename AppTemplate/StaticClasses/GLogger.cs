using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTemplate {
    public static class GLogger {
        public static Logger Logger { get; private set; }

        static GLogger() {
            Logger = new Logger();
            Logger.AddLog(new Log("Test log", LogType.Success));
        }
    }
}
