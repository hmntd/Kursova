using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.ViewModels
{
    public class MainPageUserViewModel: ViewModel
    {
        public User currentUser { get; }
        public ObservableCollection<Application> Applications { get; }
        public MainPageUserViewModel(User user)
        {
            currentUser = user;
            Applications = Data.LoadData<Application>(applicationFilePath);
        }
    }
}
