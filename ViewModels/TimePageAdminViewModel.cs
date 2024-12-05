using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ReestrForm.View.AdminWindow;
using static System.Net.Mime.MediaTypeNames;
using ReestrForm.Viev.AdminWindow;

namespace ReestrForm.ViewModels
{
    public class TimePageAdminViewModel: ViewModel
    {
        public User currentUser { get; }
        private Page _page;
        private Window _window;
        public ObservableCollection<Rate> Rates { get; set;  }
        private Rate _selectedRate;
        public Rate SelectedRate
        {
            get => _selectedRate;
            set
            {
                _selectedRate = value;
                OnPropertyChanged();
            }
        }
        public ICommand Games_Click { get; }
        public ICommand Foods_Click { get; }
        public ICommand Users_Click { get; }
        public ICommand Exit_Click { get; }
        public ICommand Stat_Click { get; }
        public ICommand Edit_Click { get; }
        public ICommand Create_Click { get; }
        public ICommand Delete_Click { get; }
        public TimePageAdminViewModel(User currentUser, Page page, Window window)
        {
            this.currentUser = currentUser;
            _page = page;
            _window = window;
            Rates = Data.LoadData<Rate>(rateFilePath);
            Games_Click = new RelayCommand(Games);
            Foods_Click = new RelayCommand(Food);
            Users_Click = new RelayCommand(Users);
            Exit_Click = new RelayCommand(Exit);
            Stat_Click = new RelayCommand(Stat);
            Edit_Click = new RelayCommand(EditRate);
            Create_Click = new RelayCommand(CreateRate);
            Delete_Click = new RelayCommand(DeleteRate);
        }
        private void Games()
        {
            MainAdminWindow mainWindow = new MainAdminWindow();
            mainWindow.DataContext = new MainAdminViewModel(currentUser, mainWindow);
            mainWindow.Show();
            Window.GetWindow(_page).Close();
        }
        private void Food()
        {
            Frame mainFrame = new Frame();
            Page page = new FoodPageAdmin();
            page.DataContext = new FoodAdminViewModel(currentUser, _window, page);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void Users()
        {
            Frame mainFrame = new Frame();
            Page page = new AdminUsersControl();
            page.DataContext = new AdminUsersControlViewModel(currentUser, _window, page);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void Exit()
        {
            var confirmViewModel = new ConfirmViewModel("Ви хочете покинути аккаунт?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result == true)
            {
                MainWindow window = new MainWindow();
                window.Show();
                _window.Close();
            }
        }
        private void Stat()
        {
            TimeStatAdmin win = new TimeStatAdmin();
            TimeStatViewModel vm = new TimeStatViewModel(Rates, win);
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void EditRate()
        {
            if (SelectedRate == null)
            {
                return;
            }

            var win = new AddTimeWindow();
            var vm = new AddTimeViewModel(SelectedRate, win, "Edit");
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void CreateRate()
        {
            var win = new AddTimeWindow();
            var vm = new AddTimeViewModel(new Rate(), win, "Create");
            win.DataContext = vm;
            win.ShowDialog();
            Rates = Data.LoadData<Rate>(rateFilePath);
        }
        private void DeleteRate()
        {
            if (SelectedRate == null)
            {
                return;
            }

            var win = new Confirm();
            var viewmodel = new ConfirmViewModel($"Ви хочете видалити {SelectedRate.Name}?");
            win.DataContext = viewmodel;
            bool? result = win.ShowDialog();
            if (result == false)
            {
                SelectedRate = null;
                return;
            }

            Rates.Remove(SelectedRate);
            Data.SaveData(rateFilePath, Rates);
            SelectedRate = null;
        }
    }
}
