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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReestrForm.ViewModels
{
    public class MainAdminViewModel: ViewModel
    {
        public User currentUser { get; }
        private Window _window;
        private ObservableCollection<ReestrForm.Models.Application> Applications;
        private ObservableCollection<ReestrForm.Models.Application> apps;
        public ObservableCollection<Models.Application> Apps
        {
            get { return apps; }
            set
            {
                apps = value;
                OnPropertyChanged(nameof(Apps));
            }
        }
        private Models.Application selectedGame;
        public Models.Application SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }
        public ICommand Exit_Click { get; }
        public ICommand Filter_All { get; }
        public ICommand Filter_Games { get; }
        public ICommand Filter_Applications { get; }
        public ICommand Stat_Click { get; }
        public ICommand TimePage_Click { get; }
        public ICommand FoodPage_Click { get; }
        public ICommand UsersPage_Click { get; }
        public ICommand EditGame_Click { get; }
        public ICommand CreateGame_Click { get; }
        public ICommand DeleteGame_Click { get; }
        public MainAdminViewModel(User user, Window window)
        {
            currentUser = user;
            this._window = window;
            Applications = Data.LoadData<ReestrForm.Models.Application>(applicationFilePath);
            Apps = Applications;
            Exit_Click = new RelayCommand(Exit);
            Filter_All = new RelayCommand(() => Filter("all"));
            Filter_Games = new RelayCommand(() => Filter("Game"));
            Filter_Applications = new RelayCommand(() => Filter("App"));
            Stat_Click = new RelayCommand(Stat);
            TimePage_Click = new RelayCommand(TimePage);
            FoodPage_Click = new RelayCommand(FoodPage);
            UsersPage_Click = new RelayCommand(UserPage);
            EditGame_Click = new RelayCommand(EditGame);
            CreateGame_Click = new RelayCommand(CreateGame);
            DeleteGame_Click = new RelayCommand(DeleteGame);
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
        private void Filter(string type)
        {
            if (type == "all")
            {
                Apps = Applications;
                return;
            }

            Apps = new ObservableCollection<Models.Application>();
            foreach (var item in Applications)
            {
                if (item.Type == type)
                {
                    Apps.Add(item);
                }
            }
        }
        private void Stat()
        {
            GameStatAdmin win = new GameStatAdmin();
            GameStatViewModel vm = new GameStatViewModel(Applications, win);
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void TimePage()
        {
            Frame mainFrame = new Frame();
            Page page = new TimePageAdmin();
            page.DataContext = new TimePageAdminViewModel(currentUser, page, _window);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void FoodPage()
        {
            Frame mainFrame = new Frame();
            Page page = new FoodPageAdmin();
            page.DataContext = new FoodAdminViewModel(currentUser, _window, page);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void UserPage()
        {
            Frame mainFrame = new Frame();
            Page page = new AdminUsersControl();
            page.DataContext = new AdminUsersControlViewModel(currentUser, _window, page);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void EditGame()
        {
            if (SelectedGame == null)
            {
                return;
            }

            var win = new AddGamesWindow();
            var vm = new AddGamesViewModel(SelectedGame, win, "Edit");
            win.DataContext = vm;
            win.ShowDialog();
        }
        private void CreateGame()
        {
            var win = new AddGamesWindow();
            var vm = new AddGamesViewModel(new Models.Application(), win, "Create");
            win.DataContext = vm;
            win.ShowDialog();
            Apps = Data.LoadData<Models.Application>(applicationFilePath);
            this.Applications = Apps;
        }
        private void DeleteGame()
        {
            if (SelectedGame == null)
            {
                return;
            }

            var win = new Confirm();
            var viewmodel = new ConfirmViewModel($"Ви хочете видалити {SelectedGame.Name}?");
            win.DataContext = viewmodel;
            bool? result = win.ShowDialog();
            if (result == false)
            {
                SelectedGame = null;
                return;
            }

            Applications.Remove(SelectedGame);
            Data.SaveData(applicationFilePath, Applications);
            Apps = Data.LoadData<Models.Application>(applicationFilePath);
            this.Applications = Apps;
            SelectedGame = null;
        }
    }
}
