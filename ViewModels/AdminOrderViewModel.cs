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
    public class AdminOrderViewModel: ViewModel
    {
        public ObservableCollection<Order> Orders { get; }
        public ObservableCollection<Order> Ords { get; set; }
        public ObservableCollection<Order> ViewOrders { get; }
        private Window _window;
        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                }

                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
        public ICommand Close_Click { get; }
        public ICommand Decline_Click { get; }
        public ICommand Accept_Click { get; }
        public AdminOrderViewModel(Window window)
        {
            Orders = Data.LoadData<Order>(orderFilePath);
            Ords = new ObservableCollection<Order>(Orders.Where(o => !o.Is_Did));
            _window = window;
            Close_Click = new RelayCommand(Close);
            Decline_Click = new RelayCommand(Decline);
            Accept_Click = new RelayCommand(Accept);
        }
        private void Close()
        {
            _window.Close();
        }
        private void Decline()
        {
            if (SelectedOrder == null)
            {
                return;
            }

            var confirmViewModel = new ConfirmViewModel($"Замовлення від {SelectedOrder.Client_Name}: {SelectedOrder.Suply_Name} у кількості {SelectedOrder.Count}\nХочете його відхилити?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result == true)
            {
                Orders.Remove(SelectedOrder);
                Data.SaveData(orderFilePath, Orders);
                Ords = new ObservableCollection<Order>(Orders.Where(o => !o.Is_Did));
                OnPropertyChanged(nameof(Ords));
                return;
            }

            SelectedOrder = null;
        }
        private void Accept()
        {
            if (SelectedOrder == null)
            {
                return;
            }

            var confirmViewModel = new ConfirmViewModel($"Замовлення від {SelectedOrder.Client_Name}: {SelectedOrder.Suply_Name} у кількості {SelectedOrder.Count}\nБуло виконано?");
            var confirmWindow = new Confirm { DataContext = confirmViewModel };
            bool? result = confirmWindow.ShowDialog();
            if (result != true)
            {
                _window.Close();
                return;
            }

            SelectedOrder.Is_Did = true;
            Data.SaveData(orderFilePath, Orders);
            Ords = new ObservableCollection<Order>(Orders.Where(o => !o.Is_Did));
            OnPropertyChanged(nameof(Ords));
            var suplies = Data.LoadData<Suply>(suplyFilePath); 
            var suply = suplies.FirstOrDefault(s => s.Name == SelectedOrder.Suply_Name);
            suply.WasBought += SelectedOrder.Count;
            Data.SaveData(suplyFilePath, suplies);
        }
    }
}
