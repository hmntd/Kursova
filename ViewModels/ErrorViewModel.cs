using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class ErrorViewModel: ViewModel
    {
        public string Message { get; set; }
        private Window _window;
        public ICommand Close_Click { get; }
        public ErrorViewModel(string message, Window window)
        {
            Message = message;
            _window = window;
            Close_Click = new RelayCommand(Close);
        }
        private void Close()
        {
            _window.Close();
        }
    }
}
