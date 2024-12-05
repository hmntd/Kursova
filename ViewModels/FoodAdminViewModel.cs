using ReestrForm.Models;
using ReestrForm.Viev.AdminWindow;
using ReestrForm.View.AdminWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace ReestrForm.ViewModels
{
    public class FoodAdminViewModel: ViewModel
    {
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
        private ObservableCollection<Suply> sup { get; }
        private ObservableCollection<Suply> suplies;
        public ObservableCollection<Suply> Suplies
        {
            get { return suplies; }
            set
            {
                suplies = value;
                OnPropertyChanged(nameof(Suplies));
            }
        }
        private Suply _selectedFood;
        public Suply SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                _selectedFood = value;
                OnPropertyChanged(nameof(SelectedFood));
            }
        }
        public ICommand Games_Click { get; }
        public ICommand Times_Click { get; }
        public ICommand Users_Click { get; }
        public ICommand Exit_Click { get; }
        public ICommand Filter_All { get; }
        public ICommand Filter_Food { get; }
        public ICommand Filter_Drink { get; }
        public ICommand Stat_Click { get; }
        public ICommand Edit_Click { get; }
        public ICommand Create_Click { get; }
        public ICommand Delete_Click { get; }
        public ICommand MinimizeWindow_Click { get; }
        public ICommand CloseWindow_Click { get; }
        public ICommand ToggleWindow_Click { get; }
        public ICommand UsersOrders_Click { get; }
        public FoodAdminViewModel(User currentUser, Window window, Page page)
        {
            this.currentUser = currentUser;
            _window = window;
            _page = page;
            this.sup = Data.LoadData<Suply>(suplyFilePath);
            Suplies = this.sup;
            OrdersCount = Data.LoadData<Order>(orderFilePath).Count(o => !o.Is_Did);
            Games_Click = new RelayCommand(Games);
            Times_Click = new RelayCommand(Times);
            Users_Click = new RelayCommand(Users);
            Exit_Click = new RelayCommand(Exit);
            Filter_All = new RelayCommand(() => Filter("all"));
            Filter_Food = new RelayCommand(() => Filter("Food"));
            Filter_Drink = new RelayCommand(() => Filter("Drink"));
            Stat_Click = new RelayCommand(Stat);
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
        public void Filter(string type)
        {
            if (type == "all")
            {
                Suplies = sup;
                return;
            }

            Suplies = new ObservableCollection<Suply>();
            foreach (var item in sup)
            {
                if (item.Type == type)
                {
                    Suplies.Add(item);
                }
            }
        }
        private void Games()
        {
            MainAdminWindow mainWindow = new MainAdminWindow();
            mainWindow.DataContext = new MainAdminViewModel(currentUser, mainWindow);
            mainWindow.Show();
            Window.GetWindow(_page).Close();
        }
        private void Times()
        {
            Frame mainFrame = new Frame();
            Page page = new TimePageAdmin();
            page.DataContext = new TimePageAdminViewModel(currentUser, page, _window);
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
            FoodStatAdmin win = new FoodStatAdmin();
            FoodStatViewModel vm = new FoodStatViewModel(this.sup, win);
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void Edit()
        {
            if (SelectedFood == null)
            {
                return;
            }

            var win = new AddFoodWindow();
            var vm = new AddFoodViewModel(SelectedFood, win, "Edit");
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void Create()
        {
            var win = new AddGamesWindow();
            var vm = new AddGamesViewModel(new Models.Application(), win, "Create");
            win.DataContext = vm;
           win.ShowDialog();
            this.Suplies = Data.LoadData<Models.Suply>(suplyFilePath);
        }
        private void Delete()
        {
            if (SelectedFood == null)
            {
                return;
            }

            var win = new Confirm();
            var viewmodel = new ConfirmViewModel($"Ви хочете видалити {SelectedFood.Name}?");
            win.DataContext = viewmodel;
            bool? result = win.ShowDialog();
            if (result == false)
            {
                SelectedFood = null;
                return;
            }

            Suplies.Remove(SelectedFood);
            Data.SaveData(suplyFilePath, Suplies);
            Suplies = Data.LoadData<Models.Suply>(applicationFilePath);
            SelectedFood = null;
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
