using Logging;
using System;

namespace ConsoleUITest {
    public class Program {
        static Logger logg;
        public static void Main(string[] args) {
            logg = new Logger() { WriteIntoConsoleOutput = true };
            logg.AddLog(new Log("test", LogType.Success));
            logg.AddLog(new Log("test2", LogType.Error  ));
            A.B.a();

            Console.WriteLine(logg.GetShortLogs());
        }

        public static class A {
            public static class B {
                public static void a() {
                    logg.AddLog(new Log("as"));
                }
            }


        
        
        }

    }
}
