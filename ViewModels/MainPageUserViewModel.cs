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
    public class MainPageUserViewModel: ViewModel
    {
        public User currentUser { get; }
        private ObservableCollection<ReestrForm.Models.Application> Applications;
        public ObservableCollection<Models.Application> Games { get; }
        public ObservableCollection<ReestrForm.Models.Application> Apps { get; }
        private Window _window;
        public ICommand TimePage_Click { get; }
        public ICommand FoodPage_Click { get; }
        public MainPageUserViewModel(User user, Window window)
        {
            currentUser = user;
            _window = window;
            Applications = Data.LoadData<ReestrForm.Models.Application>(applicationFilePath);
            Games = new ObservableCollection<ReestrForm.Models.Application>();
            Apps = new ObservableCollection<ReestrForm.Models.Application>();
            foreach (var item in Applications)
            {
                if (item.Type == "Game")
                {
                    Games.Add(item);
                }

                if (item.Type == "Application")
                {
                    Apps.Add(item);
                }
            }
            TimePage_Click = new RelayCommand(TimePage);
            FoodPage_Click = new RelayCommand(FoodPage);
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
    }
}
