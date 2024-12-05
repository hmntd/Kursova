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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReestrForm.ViewModels
{
    public class MainPageUserViewModel: ViewModel
    {
        public User currentUser { get; private set; }
        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }
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
        private Window _window;
        public ICommand TimePage_Click { get; }
        public ICommand FoodPage_Click { get; }
        public ICommand Exit_Click { get; }
        public ICommand Filter_All { get; }
        public ICommand Filter_Games { get; }
        public ICommand Filter_Applications { get; }
        public ICommand AddBalance_Click { get; }
        public ICommand StartGame_Click { get; }
        public MainPageUserViewModel(User user, Window window)
        {
            currentUser = user;
            Balance = user.Balance;
            _window = window;
            Applications = Data.LoadData<ReestrForm.Models.Application>(applicationFilePath);
            Apps = Applications;
            TimePage_Click = new RelayCommand(TimePage);
            FoodPage_Click = new RelayCommand(FoodPage);
            Exit_Click = new RelayCommand(Exit);
            Filter_All = new RelayCommand(() => Filter("all"));
            Filter_Games = new RelayCommand(() => Filter("Game"));
            Filter_Applications = new RelayCommand(() => Filter("App"));
            AddBalance_Click = new RelayCommand(AddBalance);
            StartGame_Click = new RelayCommand(startGame);
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
        private void TimePage()
        {
            Frame mainFrame = new Frame();
            Page page = new TimePageUser();
            page.DataContext = new TimePageViewModel(currentUser, page, _window);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void FoodPage()
        {
            Frame mainFrame = new Frame();
            Page page = new FoodPageUser();
            page.DataContext = new FoodPageViewModel(currentUser, page, _window);
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
        private void AddBalance()
        {
            var win = new AddBalance();
            var vm = new AddBalanceViewModel(currentUser, win);
            win.DataContext = vm;
            bool? result = win.ShowDialog();
            if (result == true)
            {
                currentUser = vm.UpdatedUser;
                Balance = currentUser.Balance;
            }
        }
        private Timer gameTimer;
        private int remainingTimeInSeconds;
        private Process gameProcess;
        private Models.Application _selectedGame;
        public Models.Application SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }
        private void startGame()
        {
            remainingTimeInSeconds = (int)(currentUser.Hours * 3600);
            try
            {
                gameProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = SelectedGame.Path_to_Application,
                        UseShellExecute = true
                    }
                };
                gameProcess.Start();
            }
            catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel($"Помилка при запуску застосунку: {ex.Message}", win);
                win.DataContext = viewModel;
                win.ShowDialog();
                return;
            }

            StartGameTimer();
        }
        private void StartGameTimer()
        {
            gameTimer = new Timer(GameTimerCallback, null, 0, 1000);
        }
        private void GameTimerCallback(object state)
        {
            if (remainingTimeInSeconds <= 0)
            {
                gameTimer.Dispose();
                currentUser.TotalHours += currentUser.Hours;
                currentUser.Hours = 0;
                SaveCurrentUser();
                OnPropertyChanged(nameof(currentUser));
                EndGame();
            }
            else
            {
                remainingTimeInSeconds--;
                currentUser.TotalHours += currentUser.Hours - (remainingTimeInSeconds / 3600);
                currentUser.Hours = remainingTimeInSeconds / 3600;
                SaveCurrentUser();
                OnPropertyChanged(nameof(currentUser));
            }
        }
        private void EndGame()
        {
            try
            {
                if (gameProcess != null && !gameProcess.HasExited)
                {
                    gameProcess.Kill();
                    gameProcess.Dispose();
                }

                SaveCurrentUser();

                var win = new Confirm();
                var viewmodel = new ConfirmViewModel("Ігровий час закінчився");
                win.DataContext = viewmodel;
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel($"помилка при зупинці гри: {ex.Message}", win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
        private void SaveCurrentUser()
        {
            try
            {
                var users = Data.LoadData<User>(userFilePath);

                var existingUser = users.FirstOrDefault(u => u.Id == currentUser.Id);
                if (existingUser != null)
                {
                    existingUser.Balance = currentUser.Balance;
                    existingUser.Hours = currentUser.Hours;
                    existingUser.TotalHours = currentUser.TotalHours;
                }

                Data.SaveData(userFilePath, users);
            }
            catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel($"Помилка при збереженні користувача: {ex.Message}", win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
    }
}
