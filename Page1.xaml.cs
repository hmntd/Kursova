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
            this.DataContext = new RegisterViewModel();
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            // Создаём новый экземпляр MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Показываем окно
            Window.GetWindow(this).Close(); // Закрываем текущую страницу
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
                    viewModel.RPassword = PasswordBox.Password;
                }
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            // Очистить текст только если он равен начальному значению
            if (textBox.Text == "Enter nickname" || textBox.Text == "Enter Email" || textBox.Text == "Enter pass" || textBox.Text == "Enter pass again")
                {
                    textBox.Clear();
                }
            }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "NickBox")
                    textBox.Text = "Enter nickname";
                else if (textBox.Name == "EmailBox")
                    textBox.Text = "Enter Email";
            }
        }
        private bool LatinAndSym(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]*$");
        }
        private bool PassValid(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9@#%&!^*.]*$");
        }
        private void GoogleLogin_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://accounts.google.com/signin") { UseShellExecute = true });
        }
        private void InstLogin_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.instagram.com/") { UseShellExecute = true });
        }
        private void TgLogin_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://web.telegram.org/k/") { UseShellExecute = true });
        }


    }
}

