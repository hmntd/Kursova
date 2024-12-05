using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using ReestrForm.View.AdminWindow;

namespace ReestrForm.ViewModels
{
    public class AdminUsersControlViewModel: ViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public User currentUser { get; }
        private Window _window;
        private Page _page;
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
        private User _selectedUser;
        public User SelectedUser
        { 
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public ICommand Games_Click { get; }
        public ICommand Times_Click { get; }
        public ICommand Foods_Click { get; }
        public ICommand Exit_Click { get; }
        public ICommand Edit_Click { get; }
        public ICommand Create_Click { get; }
        public ICommand Delete_Click { get; }
        public ICommand MinimizeWindow_Click { get; }
        public ICommand CloseWindow_Click { get; }
        public ICommand ToggleWindow_Click { get; }
        public ICommand UsersOrders_Click { get; }
        public AdminUsersControlViewModel(User user, Window window, Page page)
        {
            currentUser = user;
            Users = Data.LoadData<User>(userFilePath);
            _window = window;
            _page = page;
            OrdersCount = Data.LoadData<Order>(orderFilePath).Count(o => !o.Is_Did);
            Exit_Click = new RelayCommand(Exit);
            Games_Click = new RelayCommand(Games);
            Times_Click = new RelayCommand(Times);
            Foods_Click = new RelayCommand(Food);
            Edit_Click = new RelayCommand(Edit);
            Create_Click = new RelayCommand(Create);
            Delete_Click = new RelayCommand(Delete);
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
        private void Times()
        {
            Frame mainFrame = new Frame();
            Page page = new TimePageAdmin();
            page.DataContext = new TimePageAdminViewModel(currentUser, page, _window);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void Edit()
        {
            if (SelectedUser == null)
            {
                return;
            }

            var win = new EditUser();
            var vm = new EditUserViewModel(SelectedUser, win, "Edit");
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void Create()
        {
            var win = new EditUser();
            var vm = new EditUserViewModel(new Models.User(), win, "Create");
            win.DataContext = vm;
            win.ShowDialog();
            Users = Data.LoadData<Models.User>(userFilePath);
        }
        private void Delete()
        {
            if (SelectedUser == null)
            {
                return;
            }

            var win = new Confirm();
            var viewmodel = new ConfirmViewModel($"Ви хочете видалити {SelectedUser.Username}?");
            win.DataContext = viewmodel;
            bool? result = win.ShowDialog();
            if (result == false)
            {
                SelectedUser = null;
                return;
            }

            Users.Remove(SelectedUser);
            Data.SaveData(suplyFilePath, Users);
            Users = Data.LoadData<Models.User>(userFilePath);
            SelectedUser = null;
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
