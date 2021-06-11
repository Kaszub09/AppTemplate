using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgramTemplate {
    public class Process {
        CancellationToken? _token;
        ProcessParameters _parameters;
        IProgress<ProgressInfo> _processProgress;


        public Process(ProcessParameters parameters, CancellationToken? token = null, IProgress<ProgressInfo> processProgress = null) {
            _token = token;
            _parameters = parameters;
            _processProgress = processProgress;
        }

        public void Run() {
            var progInfo = new ProgressInfo() { AllItemsNumber = 10, Status = Status.Running, ProcessedItems = 0 };
            _processProgress?.Report(progInfo);

            //simulate work
            for (int i = 1; i <= 10; i++) {
                if (_token.HasValue && _token.Value.IsCancellationRequested == true) {
                    Console.WriteLine("CANCELATION REQUESTED");
                    break;
                }

                Thread.Sleep(500);
                progInfo.ProcessedItems = i;
                _processProgress?.Report(progInfo);
                Console.WriteLine("PROCESSING " + i);
            }

            progInfo.Status = Status.Completed;
            _processProgress?.Report(progInfo);
        }

    }
}
