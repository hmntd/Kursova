﻿using ReestrForm.Models;
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
        private Page _page;
        private Window _window;
        public ObservableCollection<Rate> Rates { get; }
        public ICommand Games_Click { get; }
        public ICommand Foods_Click { get; }
        public ICommand BuyRate_Command { get; }
        public TimePageViewModel(User user, Page page, Window window)
        {
            currentUser = user;
            this._page = page;
            _window = window;
            Games_Click = new RelayCommand(Games);
            Foods_Click = new RelayCommand(Foods);
            BuyRate_Command = new RelayCommand(() => BuyRate(SelectedRate), () => SelectedRate != null);
            Rates = Data.LoadData<Rate>(rateFilePath);
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
        private void BuyRate(Rate rate)
        {
            if (rate != null)
            {
                currentUser.Hours += rate.Hours;
                Rate? oldRate = Rates.FirstOrDefault(r => r.Id == rate.Id);
                if (oldRate != null)
                {
                    oldRate.WasBought++;
                    Data.SaveData(rateFilePath, Rates);
                }
                MessageBox.Show($"You have bouth the rate: {rate.Name}");
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
    }
}
