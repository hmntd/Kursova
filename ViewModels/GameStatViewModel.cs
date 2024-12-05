using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class GameStatViewModel: ViewModel
    {
        public ObservableCollection<Models.Application> Apps { get; }
        private Window _window;
        public ICommand Exit_Click { get; }
        public GameStatViewModel(ObservableCollection<Models.Application> applications, Window window)
        {
            Apps = applications;
            _window = window;
            Exit_Click = new RelayCommand(Exit);
        }
        private void Exit()
        {
            _window.Close();
        }
    }
}
