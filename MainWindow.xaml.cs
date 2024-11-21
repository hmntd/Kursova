using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ReestrForm.ViewModels;


namespace ReestrForm
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }
         private void Label_SignIn_Click(object sender, MouseButtonEventArgs e)
        {
            // Navigate to Page1
            Frame mainFrame = new Frame();
            mainFrame.Navigate(new Page1());
            this.Content = mainFrame;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter user name" /*|| textBox.Text == "Enter pass"*/)
            {
                textBox.Clear();
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "UserBox")
                    textBox.Text = "Enter user name";
                
                //else if (textBox.Name == "PassBox")
                //    textBox.Text = "Enter pass";
            }
        }
        private void Pass_GotFocus(object sender, RoutedEventArgs e)
        {
            // Очистить текст только если он равен начальному значению
            if (PassBox.Text == "Enter pass")
            {
                PassBox.Clear();
            }
        }
        private void Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox != null && string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PassBox.Text = "Enter pass";
            }
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
