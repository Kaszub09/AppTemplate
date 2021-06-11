using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    public enum LogType { 
        DebugInfo = 0b_0000_0001,
        Success = 0b_0000_0010,
        Error = 0b_0000_0100,
        Warning = 0b_0000_1000
    }

}
