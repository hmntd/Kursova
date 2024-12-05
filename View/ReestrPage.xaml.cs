using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using ReestrForm.Models;
using ReestrForm.ViewModels;

namespace ReestrForm
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private const string FilePath = "users.json";
        public Page1()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(this);
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as RegisterViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }

        }
        private void ConfPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var confPasswordBox = sender as PasswordBox;
            if (confPasswordBox != null)
            {
                var viewModel = this.DataContext as RegisterViewModel;
                if (viewModel != null)
                {
                    viewModel.RPassword = confPasswordBox.Password;
                }
            }
        }
    }
}

