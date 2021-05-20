using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppTemplate.View.Pages {
    public interface  INavigablePage {
          void OnNavigatedFrom();
          void OnNavigatedTo(Object parameter);
    }
}
