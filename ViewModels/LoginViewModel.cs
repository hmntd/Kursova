using ReestrForm.Models;
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
    public class LoginViewModel: ViewModel
    {
        private Window _window;
        private ObservableCollection<User> users;
        private string username;
        private string password;
        private User currentUser;
        public ICommand Login_Click { get; }
        public ICommand Label_SignIn_Click { get; }
        public ICommand GoogleLogin_Click { get; }
        public ICommand InstLogin_Click { get; }
        public ICommand TgLogin_Click { get; }
        public LoginViewModel(Window window)
        {
            this.users = Data.LoadData<User>(userFilePath);
            Login_Click = new RelayCommand(Login, () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
            Label_SignIn_Click = new RelayCommand(Label_SignIn);
            _window = window;
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
                ((RelayCommand)Login_Click).RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                ((RelayCommand)Login_Click).RaiseCanExecuteChanged();
            }
        }
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }
        private void Login()
        {
            CurrentUser = users.FirstOrDefault(u => u.Username == Username);
            if (CurrentUser != null && CurrentUser.Password == Password)
            {
                MainPageUser mainPageUser = new MainPageUser();
                mainPageUser.DataContext = new MainPageUserViewModel(CurrentUser);
                mainPageUser.Show();
                _window.Close();
            } else
            {
                MessageBox.Show("Невірний логін або пароль", "Вхід", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return;
        }
        private void Label_SignIn()
        {
            // Navigate to Page1
            Frame mainFrame = new Frame();
            mainFrame.Navigate(new Page1());
            _window.Content = mainFrame;
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
