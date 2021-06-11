using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    public struct Log {
        public string Message;
        public string Function;
        public int Verbosity;
        public LogType Type;
        public DateTime Date;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Log(string message,LogType logType = LogType.DebugInfo, int verbosity=3) {
            Message = message;
            var temp = new StackFrame(1, true).GetMethod();
            //Function = temp.DeclaringType.FullName  + "#" + temp.Name;
            Function = temp.Name;
            Verbosity = verbosity;
            Type = logType;
            Date = DateTime.Now;
        }
    }
}
