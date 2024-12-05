using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ReestrForm.ViewModels
{
    public class FoodStatViewModel: ViewModel
    {
        public ObservableCollection<Suply> Suplies { get; }
        private Window _window;
        public ICommand Exit_Click { get; }
        public FoodStatViewModel(ObservableCollection<Suply> suplies, Window window)
        {
            Suplies = suplies;
            _window = window;
            Exit_Click = new RelayCommand(Exit);
        }
        private void Exit()
        {
            _window.Close();
        }
    }
}
