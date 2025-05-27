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
                    var viewModel = new ErrorViewModel("кількість не може бути нижче за 0", win);
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
            var users = Data.LoadData<User>("users");
            var user = users.FirstOrDefault(u => u.Username == currentUser.Username);
            user.Balance = currentUser.Balance;
            Data.SaveData<User>(users, "users", "Username");
            var orders = new ObservableCollection<Order>(); // не читаємо всю таблицю
            int Id = GenerateUniqueRandomId(); // див нижче
            orders.Add(new Order(Id, Product.Name, user.Username, Count, false));
            try
            {
                Data.SaveData(orders, "orders", "Id");
            }
            catch (Exception ex)
            {
                // Логування або відображення повідомлення
                Console.WriteLine($"Error in SaveData: {ex.Message}");
            }
            
            _window.DialogResult = true;
            _window.Close();
        }
        private static readonly Random _random = new Random();

        private static int GenerateUniqueRandomId()
        {
            // Максимум 10 спроб
            for (int i = 0; i < 10; i++)
            {
                int id;
                lock (_random)
                {
                    id = _random.Next(1_000_000, 10_000_000);
                }

                // Перевіримо, чи такий Id вже існує (без завантаження всіх orders)
                string query = $"SELECT 1 FROM orders WHERE Id = {id} LIMIT 1;";
                bool exists = Data.ReadQuery(query).Any();
                if (!exists)
                    return id;
            }

            throw new Exception("Не вдалося згенерувати унікальний Id після 10 спроб.");
        }

        private class TempCheck
        {
            public int Id { get; set; }
        }
    }
}
