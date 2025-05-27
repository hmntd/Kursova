using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class TimePageViewModel: ViewModel
    {
        public User currentUser { get; set; }
        private float balance;
        public float Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }
        private string rateName;
        public string RateName
        {
            get { return rateName; }
            set
            {
                rateName = value;
                OnPropertyChanged(nameof(RateName));
            }
        }
        private float hours;
        public float Hours
        {
            get { return hours; }
            set
            {
                hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }
        private Page _page;
        private Window _window;
        public ObservableCollection<Rate> Rates { get; }
        public ICommand Games_Click { get; }
        public ICommand Foods_Click { get; }
        public ICommand BuyRate_Command { get; }
        public ICommand Exit_Click { get; }
        public ICommand AddBalance_Click { get; }
        public ICommand TgLink_Click { get; }
        public ICommand DiscordLink_Click { get; }
        public ICommand InstLink_Click { get; }
        public TimePageViewModel(User user, Page page, Window window)
        {
            currentUser = user;
            Balance = user.Balance;
            RateName = user.Rate_name;
            Hours = user.Hours;
            this._page = page;
            _window = window;
            Games_Click = new RelayCommand(Games);
            Foods_Click = new RelayCommand(Foods);
            BuyRate_Command = new RelayCommand(() => BuyRate(SelectedRate), () => SelectedRate != null);
            Rates = Data.LoadData<Rate>(rateFilePath);
            Exit_Click = new RelayCommand(Exit);
            AddBalance_Click = new RelayCommand(AddBalance);
            TgLink_Click = new RelayCommand(Tg_Link);
            DiscordLink_Click = new RelayCommand(Discord_Link);
            InstLink_Click = new RelayCommand(Inst_Link);
        }
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
        private void BuyRate(Rate rate)
        {
            var win = new Confirm();
            var viewmodel = new ConfirmViewModel($"Ви хочете купити {rate.Name} за ціною: {rate.Price}?");
            win.DataContext = viewmodel;
            bool? result = win.ShowDialog();
            if (result == false)
            {
                SelectedRate = null;
                return;
            }

            if (rate.Price > currentUser.Balance)
            {
                var win2 = new ErorWin();
                var viewModel = new ErrorViewModel("На балансі недостатньо коштів", win2);
                win2.DataContext = viewModel;
                win2.ShowDialog();
                return;
            }

            currentUser.Hours += rate.Hours;
            currentUser.Rate_name = rate.Name;
            currentUser.Balance -= rate.Price;
            var users = Data.LoadData<User>(userFilePath);
            var user = users.FirstOrDefault(u => u.Username == currentUser.Username);
            user.Hours = currentUser.Hours;
            user.Rate_name = currentUser.Rate_name;
            user.Balance = currentUser.Balance;
            Balance = user.Balance;
            RateName = user.Rate_name;
            Hours = user.Hours;
            Data.SaveData(users, "users", "Username");

            Rate? oldRate = Rates.FirstOrDefault(r => r.Name == rate.Name);
            try
            {
                if (oldRate != null)
            {
                oldRate.Bought_count++;
                Data.SaveData(Rates, "rates", "Name");
            }
            }
            catch (Exception ex)
            {
                // Логування або відображення повідомлення
                Console.WriteLine($"Error in SaveData: {ex.Message}");
            }
            
        }
        private void Games()
        {
            MainPageUser mainWindow = new MainPageUser();
            mainWindow.DataContext = new MainPageUserViewModel(currentUser, mainWindow);
            mainWindow.Show();
            Window.GetWindow(_page).Close();
        }
        private void Foods()
        {
            Frame mainFrame = new Frame();
            Page page = new FoodPageUser();
            page.DataContext = new FoodPageViewModel(currentUser, page, _window);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
        }
        private void Exit()
        {
            var confirmViewModel = new ConfirmViewModel("Are you sure to exit acc?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result == true)
            {
                MainWindow window = new MainWindow();
                window.Show();
                _window.Close();
            }
        }
    }
}
