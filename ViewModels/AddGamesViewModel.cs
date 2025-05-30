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
using System.Xml.Linq;

namespace ReestrForm.ViewModels
{
    public class AddGamesViewModel : ViewModel
    {
        public Models.Application Application { get; set; }
        public string[] TypeApplication { get; } = ["App", "Game"];
        private Window _window;
        public ICommand Exit_Click { get; }
        public ICommand Close_Click { get; }
        public ICommand FileDialog_Click { get; }
        public ICommand ImageDialog_Click { get; }
        public ICommand Save_Click { get; }
        public AddGamesViewModel(Models.Application app, Window window, string typeW)
        {
            Application = app;
            _window = window;
            Exit_Click = new RelayCommand(Exit);
            Close_Click = new RelayCommand(Close);
            FileDialog_Click = new RelayCommand(FileDialog);
            ImageDialog_Click = new RelayCommand(ImageDialog);
            Save_Click = new RelayCommand(typeW == "Create" ? Save : Edit );
        }
        private void Exit()
        {
            _window.Close();
        }
        private void FileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                this.Application.Path_to_Application = dialog.FileName;
                OnPropertyChanged(nameof(Application));
            }
        }
        private void ImageDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                this.Application.Path_to_Image = dialog.FileName;
                OnPropertyChanged(nameof(Application));
            }
        }
        private void Close()
        {
            _window.Close();
        }
        private void Save()
        {
            try
            {
                GameValidationRules.NameValidation(this.Application.Name);
                GameValidationRules.TypeValidation(this.Application.Type);
                GameValidationRules.FileExistsValidation(this.Application.Path_to_Application);
                GameValidationRules.FileExistsValidation(this.Application.Path_to_Image);
                if (this.Application.Id == 0)
                {
                    this.Application.Id = GenerateUniqueRandomId();
                }
                var apps = Data.LoadData<Models.Application>("applications");
                
                apps.Add(this.Application);

                try
                {
                    Data.SaveData(apps, "applications", "id");
                }
                catch (Exception ex)
                {
                    // Логування або відображення повідомлення
                    Console.WriteLine($"Error in SaveData: {ex.Message}");
                }
                
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
        private void Edit()
        {
            try
            {

                var apps = Data.LoadData<Models.Application>("applications");

                var existingApp = apps.FirstOrDefault(app => app.Id == this.Application.Id);
                

                existingApp.Name = this.Application.Name;
                existingApp.Type = this.Application.Type;
                existingApp.Path_to_Application = this.Application.Path_to_Application;
                existingApp.Path_to_Image = this.Application.Path_to_Image;

                try
                {
                    Data.SaveData(apps, "applications", "id");
                }
                catch (Exception ex)
                {
                    // Логування або відображення повідомлення
                    Console.WriteLine($"Error in SaveData: {ex.Message}");
                }
                

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
