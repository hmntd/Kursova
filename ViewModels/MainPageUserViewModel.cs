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
        private ObservableCollection<ReestrForm.Models.Application> apps;
        public ObservableCollection<Models.Application> Apps
        {
            get { return apps; }
            set
            {
                apps = value;
                OnPropertyChanged(nameof(Apps));
            }
        }
        private Window _window;
        public ICommand TimePage_Click { get; }
        public ICommand FoodPage_Click { get; }
        public ICommand Exit_Click { get; }
        public ICommand Filter_All { get; }
        public ICommand Filter_Games { get; }
        public ICommand Filter_Applications { get; }
        public MainPageUserViewModel(User user, Window window)
        {
            currentUser = user;
            _window = window;
            Applications = Data.LoadData<ReestrForm.Models.Application>(applicationFilePath);
            Apps = Applications;
            TimePage_Click = new RelayCommand(TimePage);
            FoodPage_Click = new RelayCommand(FoodPage);
            Exit_Click = new RelayCommand(Exit);
            Filter_All = new RelayCommand(() => Filter("all"));
            Filter_Games = new RelayCommand(() => Filter("Game"));
            Filter_Applications = new RelayCommand(() => Filter("App"));
        }
        private void Filter(string type)
        {
            if (type == "all")
            {
                Apps = Applications;
                return;
            }

            Apps = new ObservableCollection<Models.Application>();
            foreach (var item in Applications)
            {
                if (item.Type == type)
                {
                    Apps.Add(item);
                }
            }
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
        private void Exit()
        {
            var result = MessageBox.Show("Are you sure to exit acc", "Exiting", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow window = new MainWindow();
                window.Show();
                _window.Close();
            }
        }
    }
}
