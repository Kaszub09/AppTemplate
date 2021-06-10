using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppTemplate.View.Pages;

namespace AppTemplate {
    static public class PagesManager {
        public static event EventHandler PageChanged;
        public static INavigablePage CurrentPage { get; private set; }

        private static Dictionary<string,  INavigablePage> _allPages;
        private static List< INavigablePage> _navigationHistory;
        private static int _currentPageNumber;

        static PagesManager() {
            _allPages = new Dictionary<string, INavigablePage>();
            _navigationHistory = new List<INavigablePage>();
            _currentPageNumber = -1;
        }

        public static void AddPage(string key, INavigablePage page) {
            _allPages[key] = page;
        }

        public static void Navigate(string key, object parameter = null) {
            if (_allPages.ContainsKey(key)) {
                if (_navigationHistory.Count -1> _currentPageNumber) {
                    _navigationHistory.RemoveRange(_currentPageNumber+1, _navigationHistory.Count- _currentPageNumber-1);
                }

                if (CurrentPage!= _allPages[key]) {
                    _currentPageNumber++;
                    _navigationHistory.Add(_allPages[key]);
                }

                ChangePage(_allPages[key], null);
            } else {
                try {
                    Navigate(Type.GetType(key), parameter);
                } catch(Exception r) { 
                    //TO DO - LOGGER
                }
            }
        }

        public static void Navigate(Type type, object parameter = null) {
            foreach (var item in _allPages) {
                if (item.Value.GetType() == type) {
                    Navigate(item.Key, parameter);
                    return;
                }
            }

            if (type.GetInterfaces().Contains(typeof(INavigablePage))   ) {
                _allPages[type.Name] = (INavigablePage)Activator.CreateInstance(type);
                Navigate(type.Name, parameter);
            }       
        }

        public static void GoBack() {
            if (_navigationHistory.Count > 0 && _currentPageNumber>0) {
                _currentPageNumber--;
                ChangePage(_navigationHistory[_currentPageNumber], null);
            }
        }

        public static void GoForward() {
            if (_currentPageNumber<_navigationHistory.Count-1) {
                _currentPageNumber++;
                ChangePage(_navigationHistory[_currentPageNumber],null);
            }
        }

        public static bool CanGoBack() {
            if (_navigationHistory.Count > 0 && _currentPageNumber > 0) {
                return true;
            }
            return false;
        }

        public static bool CanGoForward() {
            if (_currentPageNumber < _navigationHistory.Count - 1) {
                return true;
            }
            return false;
        }

        private static void ChangePage(INavigablePage page,object parameter) {
            CurrentPage?.OnNavigatedFrom();
            page.OnNavigatedTo(parameter);
            CurrentPage = page;
            PageChanged?.Invoke(null, EventArgs.Empty);
        }

    }
}
