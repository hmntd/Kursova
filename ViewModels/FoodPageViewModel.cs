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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReestrForm.ViewModels
{
    public class FoodPageViewModel: ViewModel
    {
        public User currentUser { get; set; }
        private ObservableCollection<Suply> sup { get; }
        private ObservableCollection<Suply> suplies;
        public ObservableCollection<Suply> Suplies
        {
            get { return suplies; }
            set { 
                suplies = value;
                OnPropertyChanged(nameof(Suplies));
            }
        }
        private Page _page;
        private Window _window;
        public ICommand Games_Click { get; }
        public ICommand Times_Click { get; }
        public ICommand Buy_Food_Click { get; }
        public ICommand Filter_All {  get; }
        public ICommand Filter_Food { get; }
        public ICommand Filter_Drink { get; }
        public ICommand Exit_Click { get; }
        public FoodPageViewModel(User currentUser, Page page, Window window)
        {
            this.currentUser = currentUser;
            _page = page;
            _window = window;
            Games_Click = new RelayCommand(Games);
            Times_Click = new RelayCommand(Times);
            Filter_All = new RelayCommand(() => Filter("all"));
            Filter_Food = new RelayCommand(() => Filter("Food"));
            Filter_Drink = new RelayCommand(() => Filter("Drink"));
            sup = Data.LoadData<Suply>(suplyFilePath);
            Suplies = sup;
            Buy_Food_Click = new RelayCommand(() => BuyFood(SelectedFood), () => SelectedFood != null);
            Exit_Click = new RelayCommand(Exit);
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
            MainPageUser mainWindow = new MainPageUser();
            mainWindow.DataContext = new MainPageUserViewModel(currentUser, mainWindow);
            mainWindow.Show();
            Window.GetWindow(_page).Close();
        }
        private void Times()
        {
            Frame mainFrame = new Frame();
            Page page = new TimePageUser();
            page.DataContext = new TimePageViewModel(currentUser, page, _window);
            mainFrame.Navigate(page);
            _window.Content = mainFrame;
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
        private void BuyFood(Suply suply)
        {
            var win = new FoodConfirmPage();
            var viewmodel = new FoodConfirmViewModel(currentUser, suply, win);
            win.DataContext = viewmodel;
            win.Show();
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
