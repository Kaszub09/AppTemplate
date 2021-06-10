using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AppTemplate.Model;
using AppTemplate.View.Pages;

namespace AppTemplate.VM {
    public class VMCommands : INotifyPropertyChanged {
        //Public properties
        public event PropertyChangedEventHandler PropertyChanged;

        [IndexerName("Item")]
        public ICommand this[string key] {
            get {
                return _commands[key];
            }
        }

        //Private properties
        private Dictionary<string, ICommand> _commands;

        public VMCommands() {
            _commands = Commands.AllCommands;
            Commands.CommandsChanged += Commands_CommandsChanged;
        }

        private void Commands_CommandsChanged(object sender, EventArgs e) {
            _commands = Commands.AllCommands;
            OnPropertyChanged("Item[]");
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Destructor
        ~VMCommands() {
            Commands.CommandsChanged -= Commands_CommandsChanged;
        }
    }
}
