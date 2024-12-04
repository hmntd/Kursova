using ReestrForm.Models;
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
    public class FoodConfirmViewModel: ViewModel
    {
        private User currentUser;
        public Suply Product { get; }
        private int count = 1;
        public int Count
        {
            get { return count; }
            set
            {
                if (value < 0)
                {
                    count = 0;
                    OnPropertyChanged(nameof(Count));
                    var win = new ErorWin();
                    var viewModel = new ErrorViewModel("Count cann't be below zero", win);
                    win.DataContext = viewModel;
                    win.Show();
                }
                count = value;
                OnPropertyChanged(nameof(Count));
            }
        }
        private Window _window;
        public ICommand Buy_Click { get; }
        public ICommand Cancel_Click { get; }
        public ICommand Plus_Click { get; }
        public ICommand Minus_Click { get; }
        public FoodConfirmViewModel(User currentUser, Suply product, Window window)
        {
            this.currentUser = currentUser;
            Product = product;
            _window = window;
            Cancel_Click = new RelayCommand(Cancel);
            Buy_Click = new RelayCommand(BuyFood);
            Plus_Click = new RelayCommand(() => Count++);
            Minus_Click = new RelayCommand(() => Count--);
        }
        private void Cancel()
        {
            _window.Close();
        }
        private void BuyFood()
        {
            var confirmViewModel = new ConfirmViewModel($"Ви впевнені у покупці {Product.Name} у кількості: {Count}");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result != true)
            {
                _window.Close();
                return;
            }

            if ((Product.Price * Count) > currentUser.Balance)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel("На балансі недостатньо коштів", win);
                win.DataContext = viewModel;
                win.Show();
                return;
            }

            currentUser.Balance -= Product.Price * Count;
            var users = Data.LoadData<User>(userFilePath);
            var user = users.FirstOrDefault(u => u.Username == currentUser.Username);
            user.Balance = currentUser.Balance;
            Data.SaveData<User>(userFilePath, users);
            ObservableCollection<Order> orders = Data.LoadData<Order>(orderFilePath);
            orders.Add(new Order(Guid.NewGuid().ToString(), Product.Name, user.Username, Count));
            Data.SaveData(orderFilePath, orders);
            _window.DialogResult = true;
            _window.Close();
        }
    }
}
