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
        private Window _window;
        private string nameOwner = "Ім'я власника";
        public string NameOwner
        {
            get { return nameOwner; }
            set {
                if (value == "")
                {
                    nameOwner = "Ім'я власника";
                    OnPropertyChanged(nameof(NameOwner));
                    return;
                }

                try
                {
                    CardValidationRules.NameOwnerValidate(value);
                    nameOwner = value;
                    OnPropertyChanged(nameof(NameOwner));
                } catch (Exception ex)
                {
                    var win = new ErorWin();
                    var viewModel = new ErrorViewModel(ex.Message, win);
                    win.DataContext = viewModel;
                    win.Show();
                    nameOwner = "Ім'я власника";
                    OnPropertyChanged(nameof(NameOwner));
                }
            }
        }
        private string date = "Дата дійсності";
        public string Date
        { 
            get { return date; }
            set
            {
                if (value == "")
                {
                    date = "Дата дійсності";
                    OnPropertyChanged(nameof(Date));
                    return;
                }

                try
                {
                    CardValidationRules.ExpiryDateValidate(value);
                    date = value;
                    OnPropertyChanged(nameof(Date));
                } catch (Exception ex)
                {
                    var win = new ErorWin();
                    var viewModel = new ErrorViewModel(ex.Message, win);
                    win.DataContext = viewModel;
                    win.Show();
                    date = "Дата дійсності";
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
        private string number = "Номер картки";
        public string Number
        {
            get { return number; }
            set
            {
                if (value == "")
                {
                    number = "Номер картки";
                    OnPropertyChanged(nameof(Number));
                    return;
                }

                try
                {
                    CardValidationRules.CardNumberValidate(int.Parse(value));
                    number = value;
                    OnPropertyChanged(nameof(Number));
                } catch (Exception ex)
                {
                    var win = new ErorWin();
                    var viewModel = new ErrorViewModel(ex.Message, win);
                    win.DataContext = viewModel;
                    win.Show();
                    number = "Номер картки";
                    OnPropertyChanged(nameof(Number));
                }
            }
        }
        private string cvv = "CVV";
        public string CVV
        {
            get { return cvv; }
            set
            {
                if (value == "")
                {
                    cvv = "CVV";
                    OnPropertyChanged(nameof(CVV));
                    return;
                }

                try
                {
                    CardValidationRules.CvvValidation(int.Parse(value));
                    cvv = value;
                    OnPropertyChanged(nameof(CVV));
                } catch(Exception ex)
                {
                    var win = new ErorWin();
                    var viewModel = new ErrorViewModel(ex.Message, win);
                    win.DataContext = viewModel;
                    win.Show();
                    cvv = "CVV";
                    OnPropertyChanged(nameof(CVV));
                }
            }
        }
        public ICommand Cancel_Click { get; }
        public ICommand Accept_Click { get; }
        public ICommand GetFocus_NameOwner {  get; }
        public ICommand GetFocus_Date { get; }
        public ICommand GetFocus_Number { get; }
        public ICommand GetFocus_CVV { get; }
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
        }
        private void Cancel()
        {
            _window.Close();
        }
        private void Accept()
        {
            _window.Close();
        }
        private void cleanField(string field)
        {
            switch (field)
            {
                case "NameOwner":
                    NameOwner = string.Empty;
                    break;
                case "Date":
                    Date = string.Empty;
                    break;
                case "Number":
                    Number = string.Empty;
                    break;
                case "CVV":
                    CVV = string.Empty;
                    break;
            }
        }
    }
}
