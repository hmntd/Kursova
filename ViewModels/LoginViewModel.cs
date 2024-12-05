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
        public ICommand Forgot_Click { get; }
        public LoginViewModel(Window window)
        {
            this.users = Data.LoadData<User>(userFilePath);
            Login_Click = new RelayCommand(Login, () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
            Label_SignIn_Click = new RelayCommand(Label_SignIn);
            _window = window;
            Forgot_Click = new RelayCommand(Forgot);
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
                if (CurrentUser.IsAdmin)
                {
                    MainAdminWindow win = new MainAdminWindow();
                    win.DataContext = new MainAdminViewModel(CurrentUser, win);
                    win.Show();
                    _window.Close();
                    return;
                }

                MainPageUser mainPageUser = new MainPageUser();
                mainPageUser.DataContext = new MainPageUserViewModel(CurrentUser, mainPageUser);
                mainPageUser.Show();
                _window.Close();
            } else
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel("Невірний логін або пароль", win);
                win.DataContext = viewModel;
                win.ShowDialog();
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
        private void Forgot()
        {
            var win = new ErorWin();
            var viewModel = new ErrorViewModel("Зверніться до адміну для скинення паролю", win);
            win.DataContext = viewModel;
            win.ShowDialog();
        }
    }
}
