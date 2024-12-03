using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class ConfirmViewModel: ViewModel
    {
        public string Message { get; set; }
        public bool? Result { get; private set; }
        public ICommand Confirm_Click { get; }
        public ICommand Cancel_Click { get; }
        private TaskCompletionSource<bool> _taskCompletionSource;
        public ConfirmViewModel(string message)
        {
            Message = message;
            Confirm_Click = new RelayCommand(() => Confirm(true));
            Cancel_Click = new RelayCommand(() => Confirm(false));
        }
        private void Confirm(bool result)
        {
            Result = result;
            OnRequestClose();
        }
        public event Action RequestClose;
        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke();
        }
    }
}
