using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ProgramTemplate {
    public class Program {
        public bool IsRunning { get; private set; }

        private double _intervalMilisec;
        private TimerPlus _processTimer;
        private Timer _timeUpdate;

        private IProgress<DateTime?> _nextRun;
        private IProgress<ProgressInfo> _processProgress;

        private CancellationTokenSource _token;
        private ProcessParameters _parameters;
        private Task _process;

        public Program(ProcessParameters parameters, double intervalMilisec, IProgress<DateTime?> nextRun = null, IProgress<ProgressInfo> processProgress = null) {
            _parameters = parameters;
            _intervalMilisec = intervalMilisec;
            IsRunning = false;

            _timeUpdate = new Timer(500);
            _timeUpdate.Elapsed += _timeUpdate_Elapsed;

            _processTimer = new TimerPlus(intervalMilisec);
            _processTimer.Elapsed += _processTimer_Elapsed;

            _processProgress = processProgress;
            _nextRun = nextRun;
        }

        private void _timeUpdate_Elapsed(object sender, ElapsedEventArgs e) {
            if (_nextRun != null) {
                _nextRun.Report(_processTimer.NextRun());
            }
        }

        private void _processTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (_process != null) {
                if (_process.IsCompleted == false) {
                    return;
                }
            }

            _process = new Task(() => new Process(_parameters, _token.Token, _processProgress).Run());
            _process.Start();
            Console.WriteLine("program: " + _process.Id);
        }


        public bool Start() {
            if (IsRunning == false) {
                _token = new CancellationTokenSource();
                //Start process timer
                _processTimer.Interval = _intervalMilisec;
                _processTimer.Start();
                IsRunning = true;
                //Start info about net run timer
                _timeUpdate.Start();
                return true;
            } else {
                return false;
            }
        }

        public bool Stop() {
            if (IsRunning == true) {
                _processTimer.Stop();   //Stop timer
                _token.Cancel();    //Request proces to stop
                _process?.Wait();    //Wait for process to stop
                IsRunning = false;
                //Stop info about next run timer
                _timeUpdate.Stop();
                return true;
            } else {
                return false;
            }
        }

        public void ChangeInterval(double intervalMilisec) {
            _intervalMilisec = intervalMilisec;
        }

    }
}
