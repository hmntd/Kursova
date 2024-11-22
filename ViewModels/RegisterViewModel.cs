using ReestrForm.Models;
using ReestrForm.Models.ValidationRules;
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
        public ICommand Register_Click { get; }
        public RegisterViewModel()
        {
            users = Data.LoadData<User>(userFilePath);
            Register_Click = new RelayCommand(
                Register, 
                () => 
                    !string.IsNullOrEmpty(Username) && 
                    !string.IsNullOrEmpty(Password) && 
                    !string.IsNullOrEmpty(Email) && 
                    !string.IsNullOrEmpty(RPassword));
        }
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
                ((RelayCommand)Register_Click).RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                ((RelayCommand)Register_Click).RaiseCanExecuteChanged();

            }
        }
        public string RPassword
        {
            get { return rPassword; }
            set
            {
                rPassword = value;
                OnPropertyChanged("RPassword");
                ((RelayCommand)Register_Click).RaiseCanExecuteChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
                ((RelayCommand)Register_Click).RaiseCanExecuteChanged();
            }
        }
        public void Register()
        {
            newUser = new User(Guid.NewGuid(), Password, Username, Email, false);
            try
            {
                if (Password != RPassword)
                {
                    throw new Exception("Паролі не співпадають");
                }

                RegisterValidationRules.UsernameValidate(Username);
                RegisterValidationRules.PassswordValidate(Password);
                RegisterValidationRules.EmailValidate(Email);
                RegisterValidationRules.UserExistsValidate(newUser, users);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Некоректні дaні", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            users.Add(newUser);
            Data.SaveData(userFilePath, users);

            MessageBox.Show("Зареєстрованиq");
        }
    }
}
