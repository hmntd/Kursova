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
        private string gif_path;
        public string Gif_Path
        {
            get { return gif_path; }
            set
            {
                gif_path = value;
                OnPropertyChanged(Gif_Path);
            }
        }
        public int OrdersCount { get; set; }
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
        public ICommand MinimizeWindow_Click { get; }
        public ICommand CloseWindow_Click { get; }
        public ICommand ToggleWindow_Click { get; }
        public ICommand UsersOrders_Click { get; }
        public TimePageAdminViewModel(User currentUser, Page page, Window window)
        {
            this.currentUser = currentUser;
            _page = page;
            _window = window;
            OrdersCount = Data.LoadData<Order>(orderFilePath).Count(o => !o.Is_Did);
            Rates = Data.LoadData<Rate>(rateFilePath);
            Games_Click = new RelayCommand(Games);
            Foods_Click = new RelayCommand(Food);
            Users_Click = new RelayCommand(Users);
            Exit_Click = new RelayCommand(Exit);
            Stat_Click = new RelayCommand(Stat);
            Edit_Click = new RelayCommand(EditRate);
            Create_Click = new RelayCommand(CreateRate);
            Delete_Click = new RelayCommand(DeleteRate);
            MinimizeWindow_Click = new RelayCommand(() => Minimize_Window(window));
            CloseWindow_Click = new RelayCommand(() => Close_Window(window));
            ToggleWindow_Click = new RelayCommand(() => Toogle_Window(window));
            UsersOrders_Click = new RelayCommand(UserOrders);
            SetGif();
        }
        public Random random { get; } = new Random();
        private void SetGif()
        {
            string[] paths = [
                "/gif/akita_lie_8fps.gif",
                "/gif/ljSjEjQ.gif",
                "/gif/nyan-cat.gif",
                "/gif/oR1fkkiDPgSG.gif",
                "/gif/qiBoeLEaT.gif",
                ];
            Gif_Path = paths[random.Next(0, paths.Length)];
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
            OnPropertyChanged(nameof(Rates));
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
            OnPropertyChanged(nameof(Rates));
        }
        private void UserOrders()
        {
            var win = new AdminOrderWindow();
            var vm = new AdminOrderViewModel(win);
            win.DataContext = vm;
            win.ShowDialog();
            OrdersCount = Data.LoadData<Order>(orderFilePath).Count(o => !o.Is_Did);
            OnPropertyChanged(nameof(OrdersCount));
        }
    }
}
