using ReestrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using ReestrForm.Models.ValidationRules;

namespace ReestrForm.ViewModels
{
    public class EditUserViewModel: ViewModel
    {
        public User currentUser { get; set; }
        private Window _window;
        public ICommand Close_Click { get; }
        public ICommand Save_Click { get; }
        public EditUserViewModel(User user, Window window, string typeW)
        {
            currentUser = user;
            _window = window;
            Close_Click = new RelayCommand(Close);
            Save_Click = new RelayCommand(typeW == "Create" ? Save : Edit);
        }
        private void Close()
        {
            _window.Close();
        }
        private void Save()
        {
            try
            {
                var users = Data.LoadData<User>("users");
                RegisterValidationRules.UsernameValidate(currentUser.Username);
                RegisterValidationRules.EmailValidate(currentUser.Email);
                RegisterValidationRules.UserExistsValidate(currentUser, users);
                RegisterValidationRules.PassswordValidate(currentUser.Password);
                if (this.currentUser.Id == 0)
                {
                    this.currentUser.Id = GenerateUniqueRandomId();
                }
                users.Add(currentUser);

                try
                {
                    Data.SaveData(users, "users", "id");
                }
                catch (Exception ex)
                {
                    // Логування або відображення повідомлення
                    Console.WriteLine($"Error in SaveData: {ex.Message}");
                }
                _window.Close();
            } catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel(ex.Message, win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
        private void Edit()
        {
            try
            {
                var users = Data.LoadData<User>("users");
                RegisterValidationRules.UsernameValidate(currentUser.Username);
                RegisterValidationRules.EmailValidate(currentUser.Email);
                RegisterValidationRules.PassswordValidate(currentUser.Password);

                var existingUser = users.FirstOrDefault(app => app.Id == this.currentUser.Id);
                if (existingUser == null)
                {
                    throw new Exception("Людину не знайдено.");
                }

                existingUser.Email = currentUser.Email;
                existingUser.Password = currentUser.Password;
                existingUser.Username = currentUser.Username;
                existingUser.Balance = currentUser.Balance;

                try
                {
                    Data.SaveData(users, "users", "id");
                }
                catch (Exception ex)
                {
                    // Логування або відображення повідомлення
                    Console.WriteLine($"Error in SaveData: {ex.Message}");
                }
                


                _window.Close();
            } catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel(ex.Message, win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
        private static readonly Random _random = new Random();

        private static int GenerateUniqueRandomId()
        {
            // Максимум 10 спроб
            for (int i = 0; i < 10; i++)
            {
                int id;
                lock (_random)
                {
                    id = _random.Next(1_000_000, 10_000_000);
                }

                // Перевіримо, чи такий Id вже існує (без завантаження всіх orders)
                string query = $"SELECT 1 FROM orders WHERE Id = {id} LIMIT 1;";
                bool exists = Data.ReadQuery(query).Any();
                if (!exists)
                    return id;
            }

            throw new Exception("Не вдалося згенерувати унікальний Id після 10 спроб.");
        }

        private class TempCheck
        {
            public int Id { get; set; }
        }
    }
    }

