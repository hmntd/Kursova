using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace ReestrForm.ViewModels
{
    public class TimeStatViewModel: ViewModel
    {
        public ObservableCollection<Models.Rate> Rates { get; }
        private Window _window;
        public ICommand Exit_Click { get; }
        public TimeStatViewModel(ObservableCollection<Models.Rate> rates, Window window)
        {
            Rates = rates;
            _window = window;
            Exit_Click = new Models.RelayCommand(Exit);
        }
        private void Exit()
        {
            _window.Close();
        }
    }
}
