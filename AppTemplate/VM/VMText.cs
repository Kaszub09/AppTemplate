using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppTemplate.Model;

namespace AppTemplate.VM {
    public class VMText : INotifyPropertyChanged {
        //Public properties
        public event PropertyChangedEventHandler PropertyChanged;
        private string _startStopButtonID;
        public  string StartStopButton {
            get {
                return this[_startStopButtonID];
            }
            private set {
                _startStopButtonID = value;
                OnPropertyChanged();
            }
        }

        [IndexerName("Item")]
        public string this[string key] {
            get {
                if (_text.ContainsKey(key)) {
                    return _text[key];
                } else {
                    return "[" + key + "]";
                }
            }
            set {
                _text[key] = value;
            }
        }

        //Private properties
        private Dictionary<string, string> _text;

        //


        //Public methods
        public VMText() {
            StartStopButton = "button_actions_process_run";
            Subtitles.LanguageChanged += Subtitles_LanguageChanged;
            Subtitles_LanguageChanged(this, EventArgs.Empty);

            ProgramModel.StartStopStateChange += ProgramModel_StartStopStateChange;
        }

        private void ProgramModel_StartStopStateChange(object sender, CurrentState e) {
            if (e == CurrentState.Running) {
                StartStopButton = "button_actions_process_stop";
            } else {
                StartStopButton = "button_actions_process_run";
            }
        }

        //Private methods
        private void Subtitles_LanguageChanged(object sender, EventArgs e) {
            _text = Subtitles.Text;
            OnPropertyChanged("Item[]");
            StartStopButton = _startStopButtonID;
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Destructor
        ~VMText() {
            Subtitles.LanguageChanged -= Subtitles_LanguageChanged;
            ProgramModel.StartStopStateChange -= ProgramModel_StartStopStateChange;
        }
    }
}
