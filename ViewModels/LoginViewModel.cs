﻿using ReestrForm.Models;
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
    public class LoginViewModel: ViewModel
    {
        private ObservableCollection<User> users;
        private string username;
        private string password;
        private User currentUser;
        public ICommand Login_Click { get; }
        public LoginViewModel()
        {
            this.users = Data.LoadData<User>(userFilePath);
            Login_Click = new RelayCommand(Login, () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
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
                MessageBox.Show("Ви ввійшли до системи");
            } else
            {
                MessageBox.Show("Невірний логін або пароль", "Вхід", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return;
        }
    }
}
