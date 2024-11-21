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
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            // Создаём новый экземпляр MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Показываем окно
            Window.GetWindow(this).Close(); // Закрываем текущую страницу
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
        private void Pass_GotFocus(object sender, RoutedEventArgs e)
        {
              // Очистить текст только если он равен начальному значению
            if (PasswBox.Text == "Enter pass")
            {
                PasswBox.Clear();
            }
        }
        private void ConfPass_GotFocus(object sender, RoutedEventArgs e)
        {
            // Очистить текст только если он равен начальному значению
            if (ConfPassBox.Text == "Enter pass again")
            {
                ConfPassBox.Clear();
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
        private void Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox != null && string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                    PasswBox.Text = "Enter pass";
            }
        }
        private void ConfPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ConfPasswordBox != null && string.IsNullOrWhiteSpace(ConfPasswordBox.Password))
            {
                ConfPassBox.Text = "Enter pass again";
            }
        }
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string nickname = NickBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfPasswordBox.Password;

            // Проверка на только латинские символы
            if (!LatinAndSym(nickname))
            {
                MessageBox.Show("Nickname має містити лише латинські символи та цифри!");
                return;
            }
            if (!PassValid(password) || !PassValid(confirmPassword))
            {
                MessageBox.Show("Пароль мае містити лише латиницю, цифри або спец. символи!");
                return;
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не співпадають!");
                return;
            }

            var newUser = new User
            {
                Username = nickname,
                Password = password
            };

            // Загружаем существующие записи из файла JSON
            List<User> users;
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                try
                {
                    users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                }
                catch (JsonException)
                {
                    users = new List<User>();  // Обработка случая поврежденного JSON
                }
            }
            else
            {
                users = new List<User>();
            }

            // Добавляем нового пользователя в список
            users.Add(newUser);

            // Сохраняем обновленный список в файл JSON
            var updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, updatedJson);
            MessageBox.Show("Користувача зареєстровано!");
        }

        private bool LatinAndSym(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]*$");
        }
        private bool PassValid(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9@#%&!^*.]*$");
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignIn_Click(sender, e);
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

