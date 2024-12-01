using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class FoodPageViewModel
    {
        public User currentUser { get; set; }
        private Page _page;
        private Window _window;
        public ICommand Games_Click { get; }
        public ICommand Times_Click { get; }
        public FoodPageViewModel(User currentUser, Page page, Window window)
        {
            this.currentUser = currentUser;
            _page = page;
            _window = window;
            Games_Click = new RelayCommand(Games);
            Times_Click = new RelayCommand(Times);
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
    }
}
