using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.ViewModels
{
    abstract public class ViewModel: INotifyPropertyChanged
    {
        public const string userFilePath = "Data\\users.json";
        public const string applicationFilePath = "Data\\applications.json";
        public const string rateFilePath = "Data\\rates.json";
        public const string suplyFilePath = "Data\\suplies.json";
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
