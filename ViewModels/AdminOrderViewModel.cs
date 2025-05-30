using ReestrForm.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class AdminOrderViewModel : ViewModel
    {
        // Оригінальні дані
        public ObservableCollection<Order> Orders { get; private set; }
        public ObservableCollection<Order> Ords { get; private set; }

        // Колекція для відображення у UI
        public ObservableCollection<OrderDisplayItem> DisplayOrders { get; private set; }

        private Window _window;

        private OrderDisplayItem _selectedOrder;
        public OrderDisplayItem SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
            }
        }

        public ICommand Close_Click { get; }
        public ICommand Decline_Click { get; }
        public ICommand Accept_Click { get; }

        // Клас для відображення в UI (з іменами юзера і постачальника)
        public class OrderDisplayItem
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string SuplyName { get; set; }
            public int Count { get; set; }
            public bool Complited { get; set; }
        }

        public AdminOrderViewModel(Window window)
        {
            _window = window;

            // Завантажуємо всі замовлення, користувачів та постачальників
            Orders = new ObservableCollection<Order>(Data.LoadData<Order>("orders"));
            Ords = new ObservableCollection<Order>(Orders.Where(o => !o.Complited));

            UpdateDisplayOrders();

            Close_Click = new RelayCommand(Close);
            Decline_Click = new RelayCommand(Decline);
            Accept_Click = new RelayCommand(Accept);
        }

        private void UpdateDisplayOrders()
        {
            var users = Data.LoadData<User>("users");
            var suplies = Data.LoadData<Suply>("suplies");

            var displayOrders = Ords.Select(o =>
            {
                var user = users.FirstOrDefault(u => u.Id == o.Client_Name);
                var suply = suplies.FirstOrDefault(s => s.Id == o.Suply_Name);

                return new OrderDisplayItem
                {
                    Id = o.Id,
                    Username = user?.Username ?? "Unknown User",
                    SuplyName = suply?.Name ?? "Unknown Suply",
                    Count = o.Count,
                    Complited = o.Complited
                };
            });

            DisplayOrders = new ObservableCollection<OrderDisplayItem>(displayOrders);
            OnPropertyChanged(nameof(DisplayOrders));
        }

        private void Close()
        {
            _window.Close();
        }

        private void Decline()
        {
            if (SelectedOrder == null)
                return;

            // Знаходимо оригінальне замовлення за Id
            var order = Orders.FirstOrDefault(o => o.Id == SelectedOrder.Id);
            if (order == null) return;

            var confirmViewModel = new ConfirmViewModel($"Замовлення від {SelectedOrder.Username}: {SelectedOrder.SuplyName} у кількості {SelectedOrder.Count}\nХочете його відхилити?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result == true)
            {
                try
                {
                    Data.DeleteData<Order>("orders", "Id", order.Id);
                    Orders.Remove(order);
                    Ords.Remove(order);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DeleteData: {ex.Message}");
                }

                UpdateDisplayOrders();

                SelectedOrder = null;
            }
        }

        private void Accept()
        {
            if (SelectedOrder == null)
                return;

            var order = Orders.FirstOrDefault(o => o.Id == SelectedOrder.Id);
            if (order == null) return;

            var confirmViewModel = new ConfirmViewModel($"Замовлення від {SelectedOrder.Username}: {SelectedOrder.SuplyName} у кількості {SelectedOrder.Count}\nБуло виконано?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result != true)
            {
                return;
            }

            order.Complited = true;
            Data.SaveData(Orders, "orders", "Id");
            Ords = new ObservableCollection<Order>(Orders.Where(o => !o.Complited));

            var suplies = Data.LoadData<Suply>("suplies");
            var suply = suplies.FirstOrDefault(s => s.Id == order.Suply_Name);
            if (suply != null)
            {
                suply.Bought_count += order.Count;
                try
                {
                    Data.SaveData(suplies, "suplies", "Id");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in SaveData: {ex.Message}");
                }
            }

            UpdateDisplayOrders();

            SelectedOrder = null;
        }
    }
}
