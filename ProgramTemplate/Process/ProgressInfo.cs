using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramTemplate {
    public enum Status {
        Running,
        Completed
    }


    public struct ProgressInfo {
        public int ProcessedItems;
        public int AllItemsNumber;
        public Status Status;
    }
}
