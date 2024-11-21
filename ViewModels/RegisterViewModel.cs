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
    public class RegisterViewModel: ViewModel
    {
        private ObservableCollection<User> users;
        public User newUser {  get; set; }
        private string username;
        private string password;
        private string email;
        private string rPassword;
        public ICommand Register_Click;
        public RegisterViewModel()
        {
            users = Data.LoadData<User>(userFilePath);
            Register_Click = new RelayCommand(Register, () => Username != null && Password != null && Email != null && RPassword != null);
        }
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string RPassword
        {
            get { return rPassword; }
            set
            {
                rPassword = value;
                OnPropertyChanged("RPassword");
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        public void Register()
        {
            newUser = new User(Guid.NewGuid(), Password, Username, Email, false);
            try
            {
                // Validation
                if (Password != RPassword)
                {
                    throw new Exception("Паролі не співпадають");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Некоректні дaні", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            users.Add(newUser);
            Data.SaveData(userFilePath, users);

            // Future logic
            MessageBox.Show("Зареєстрованиq");
        }
    }
}
