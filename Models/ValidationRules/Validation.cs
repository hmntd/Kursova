using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    abstract public class Validation
    {
        public abstract bool Validate(string input = null, ObservableCollection<User> users = null, User user = null);
    }
}
