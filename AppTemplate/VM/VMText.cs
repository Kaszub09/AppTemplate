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

        [IndexerName("Item")]
        public string this[string key] {
            get {
                if (_text.ContainsKey(key)) {
                    return _text[key];
                } else {
                    return "[" + key + "]";
                }
            }
        }

        //Private properties
        private Dictionary<string, string> _text;

        //Public methods
        public VMText() {
            Subtitles.LanguageChanged += Subtitles_LanguageChanged;
            Subtitles_LanguageChanged(this, EventArgs.Empty);
        }


        //Private methods
        private void Subtitles_LanguageChanged(object sender, EventArgs e) {
            _text = Subtitles.CurrentLanguage;
            OnPropertyChanged("Item[]");
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Destructor
        ~VMText() {
            Subtitles.LanguageChanged -= Subtitles_LanguageChanged;
        }
    }
}
