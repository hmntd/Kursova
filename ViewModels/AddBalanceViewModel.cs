using Newtonsoft.Json.Linq;
using ReestrForm.Models;
using ReestrForm.Models.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class AddBalanceViewModel: ViewModel
    {
        private User currentUser;
        public User UpdatedUser => currentUser;
        private Window _window;
        private string nameOwner = "Ім'я власника";
        public string NameOwner
        {
            get { return nameOwner; }
            set
            {
                nameOwner = value;
                OnPropertyChanged(nameof(NameOwner));
            }
        }
        private string date = "Дата дійсності";
        public string Date
        { 
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        private string number = "Номер картки";
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        private string cvv = "CVV";
        public string CVV
        {
            get { return cvv; }
            set
            {
                cvv = value;
                OnPropertyChanged(nameof(CVV));
            }
        }
        private string sum = "Введіть сумму";
        public string Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }
        public ICommand Cancel_Click { get; }
        public ICommand Accept_Click { get; }
        public ICommand GetFocus_NameOwner {  get; }
        public ICommand GetFocus_Date { get; }
        public ICommand GetFocus_Number { get; }
        public ICommand GetFocus_CVV { get; }
        public ICommand GetFocus_Sum { get; }
        public AddBalanceViewModel(User user, Window window)
        {
            currentUser = user;
            _window = window;
            Cancel_Click = new RelayCommand(Cancel);
            Accept_Click = new RelayCommand(Accept);
            GetFocus_NameOwner = new RelayCommand(() => cleanField("NameOwner"));
            GetFocus_Date = new RelayCommand(() => cleanField("Date"));
            GetFocus_Number = new RelayCommand(() => cleanField("Number"));
            GetFocus_CVV = new RelayCommand(() => cleanField("CVV"));
            GetFocus_Sum = new RelayCommand(() => cleanField("Sum"));
        }
        private void Cancel()
        {
            _window.Close();
        }
        private void Accept()
        {
            try
            {
                CardValidationRules.NameOwnerValidate(NameOwner);
                CardValidationRules.ExpiryDateValidate(Date);
                CardValidationRules.CardNumberValidate(long.Parse(Number));
                CardValidationRules.CvvValidation(int.Parse(CVV));
                CardValidationRules.SumValidate(int.Parse(Sum));

                var users = Data.LoadData<User>(userFilePath);
                var user = users.FirstOrDefault(u => u.Username == currentUser.Username);
                user.Balance += int.Parse(Sum);
                currentUser.Balance += int.Parse(Sum);
                Data.SaveData(userFilePath, users);
                _window.DialogResult = true;
                _window.Close();
            }
            catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel(ex.Message, win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
        private void cleanField(string field)
        {
            switch (field)
            {
                case "NameOwner":
                    if (NameOwner == "Ім'я власника")
                    {
                        NameOwner = string.Empty;
                    }
                    break;
                case "Date":
                    if (Date == "Дата дійсності")
                    {
                        Date = string.Empty;
                    }
                    break;
                case "Number":
                    if (Number == "Номер картки")
                    {
                        Number = string.Empty;
                    }
                    break;
                case "CVV":
                    if (CVV == "CVV")
                    {
                        CVV = string.Empty;
                    }
                    break;
                case "Sum":
                    if (Sum == "Введіть сумму")
                    {
                        Sum = string.Empty;
                    }
                    break;
            }
        }
    }
}
