using Microsoft.Win32;
using ReestrForm.Models;
using ReestrForm.Models.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    public class AddTimeViewModel: ViewModel
    {
        public Models.Rate Rate { get; set; }
        private Window _window;
        public string Name { get; set; }
        public ICommand Close_Click { get; }
        public ICommand FileDialog_Click { get; }
        public ICommand Save_Click { get; }
        public AddTimeViewModel(Models.Rate rate, Window window, string tw)
        {
            Rate = rate;
            Name = rate.Name;
            _window = window;
            Close_Click = new RelayCommand(Close);
            FileDialog_Click = new RelayCommand(FileDialog);
            Save_Click = new RelayCommand(tw == "Create" ? Save : Edit);
        }
        private void Close()
        {
            _window.Close();
        }
        private void FileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                Rate.Path_to_image = dialog.FileName;
                OnPropertyChanged(nameof(Rate));
            }
        }
        private void Save()
        {
            try
            {

                RateValidationRules.NameValidation(Name);
                RateValidationRules.PriceValidation(Rate.Price);
                RateValidationRules.FileExistsValidation(Rate.Path_to_image);
                if (this.Rate.Id == 0)
                {
                    this.Rate.Id = GenerateUniqueRandomId();
                }
                var rates = Data.LoadData<Models.Rate>("rates");
                Rate.Name = Name;
                rates.Add(this.Rate);
                Data.SaveData(rates, "rates", "id");
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
                RateValidationRules.PriceValidation(Rate.Price);
                RateValidationRules.FileExistsValidation(Rate.Path_to_image);
                RateValidationRules.HoursValidation(Rate.Hours);

                var rates = Data.LoadData<Models.Rate>("rates");

                var existingRate = rates.FirstOrDefault(r => r.Name == this.Rate.Name);
                if (existingRate == null)
                {
                    throw new Exception("Тарифу не знайдено");
                }

                existingRate.Name = Name;
                existingRate.Price = this.Rate.Price;
                existingRate.Path_to_image = this.Rate.Path_to_image;
                existingRate.Hours = Rate.Hours;

                Data.SaveData(rates, "rates", "id");

                _window.Close();
            }
            catch (Exception ex)
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
