using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.ViewModels
{
    public class MainPageUserViewModel: ViewModel
    {
        public User currentUser { get; }
        public MainPageUserViewModel(User user)
        {
            currentUser = user;
            // comm
        }
    }
}
