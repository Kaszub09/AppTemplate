using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Logging {
    public class Logger {
        public List<Log> AllLogs { get; private set; }
        public string LogsFilepath { get; private set; }
        public bool WriteIntoDebugOutput=false;
        public bool WriteIntoConsoleOutput = false;
        private int _lastReadLogIndex;

        public Logger() {
            _lastReadLogIndex = -1;
            AllLogs = new List<Log>();
            ChangeFilepath(@"data\" + DateTime.Now.ToString("yyyy_MM_dd hh-mm-ss") +  ".txt");
            AddLog(new Log("========== START  ========== "));
        }

        #region log formatting
        public string GetLogAsString(Log log, bool date = true, bool type = true, bool func = true, bool verbosity = true) {
            var text = (date ? log.Date.ToString("dd-MM-yyyy hh:mm:ss") + " | " : null)
                 + (type ? log.Type.ToString().PadRight(9) : null)
                 + (verbosity ? log.Verbosity.ToString() : null)
                 + "-> "
                 + (func ? log.Function + ": " : null)
                 + log.Message;
            return text;
        }

        public string GetShortLog(Log log) {
            var text = log.Date.ToString("hh:mm:ss") + "|" + log.Type.ToString()[0] + " -> " + log.Message;
            return text;
        }
        #endregion log formatting

        public string GetShortLogs(LogType type = LogType.Success| LogType.Warning| LogType.Error
            , bool moveLogPointer = true,int maxVerbosity = 3) {
            if(AllLogs.Count > _lastReadLogIndex + 1) {
                var logs = AllLogs.GetRange(_lastReadLogIndex + 1, AllLogs.Count - _lastReadLogIndex - 1)
                .Where(l => l.Verbosity <= maxVerbosity && type.HasFlag(l.Type))
                .Select(l => GetShortLog(l));

                if(moveLogPointer)
                    _lastReadLogIndex = AllLogs.Count;

                return string.Join("\n", logs);
            } else {
                return "";
            }
        }

        public bool AddLog(Log log) {
            AllLogs.Add(log);
            var text = GetLogAsString(log);
            try {
                File.AppendAllText(LogsFilepath, text + "\n");
            } catch(Exception e) {
                return true;
            }
               
            if(WriteIntoDebugOutput)
                Debug.WriteLine(text);
            if (WriteIntoConsoleOutput)
                Console.WriteLine(text);

            return false;
        }

        public bool ChangeFilepath(string newFilepath) {
            try {
                var dir = Path.GetDirectoryName(newFilepath);
                if (Directory.Exists(dir) == false) {
                    Directory.CreateDirectory(dir);
                }
                LogsFilepath = newFilepath;
            } catch(Exception e) {
                AddLog(new Log(e.Message + "\nSTACK: "+ e.StackTrace, LogType.Error));
                return false;
            }
            return true;
        }


    }
}
