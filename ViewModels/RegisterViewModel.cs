using ReestrForm.Models;
using ReestrForm.Models.ValidationRules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class RegisterViewModel: ViewModel
    {
        private Page _page;
        private ObservableCollection<User> users;
        public User newUser {  get; set; }
        private string username;
        private string password;
        private string email;
        private string rPassword;
        public ICommand Register_Click { get; }
        public ICommand Back_Click { get; }
        public ICommand GoogleLogin_Click { get; }
        public ICommand InstLogin_Click { get; }
        public ICommand TgLogin_Click { get; }

        public RegisterViewModel(Page page)
        {
            _page = page;
            users = Data.LoadData<User>(userFilePath);
            Register_Click = new RelayCommand(
                Register, 
                () => 
                    !string.IsNullOrEmpty(Username) && 
                    !string.IsNullOrEmpty(Password) && 
                    !string.IsNullOrEmpty(Email) && 
                    !string.IsNullOrEmpty(RPassword));
            Back_Click = new RelayCommand(Back);
            GoogleLogin_Click = new RelayCommand(GoogleLogin);
            InstLogin_Click = new RelayCommand(InstLogin);
            TgLogin_Click = new RelayCommand(TgLogin);
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
            newUser = new User(Guid.NewGuid(), Password, Username, Email, false, 0, 0, 0, null);
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
                var win = new ErorWin();
                var viewModel = new ErrorViewModel(ex.Message, win);
                win.DataContext = viewModel;
                win.ShowDialog();
                return;
            }
            users.Add(newUser);
            Data.SaveData(userFilePath, users);

            MainPageUser mainPageUser = new MainPageUser();
            mainPageUser.DataContext = new MainPageUserViewModel(newUser, mainPageUser);
            mainPageUser.Show();
            Window.GetWindow(_page).Close() ;
        }
        private void Back()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(_page).Close();
        }
        private void GoogleLogin()
        {
            Process.Start(new ProcessStartInfo("https://accounts.google.com/signin") { UseShellExecute = true });
        }
        private void InstLogin()
        {
            Process.Start(new ProcessStartInfo("https://www.instagram.com/") { UseShellExecute = true });
        }
        private void TgLogin()
        {
            Process.Start(new ProcessStartInfo("https://web.telegram.org/k/") { UseShellExecute = true });
        }
    }
}
