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
    public class AddFoodViewModel: ViewModel
    {
        public Suply Suply { get; set; }
        public string Name { get; set; }
        public string[] TypeFood { get; } = ["Drink", "Food"];
        private Window _window;
        public ICommand Close_Click { get; }
        public ICommand FileDialog_Click { get; }
        public ICommand Save_Click { get; }
        public AddFoodViewModel(Suply suply, Window window, string typeW)
        {
            Suply = suply;
            Name = suply.Name;
            _window = window;
            Close_Click = new RelayCommand(Close);
            FileDialog_Click = new RelayCommand(FileDialog);
            Save_Click = new RelayCommand(typeW == "Create" ? Save : Edit);
        }
        private void Close()
        {
            _window.Close();
        }
        private void FileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                Suply.Path_to_Image = dialog.FileName;
                OnPropertyChanged(nameof(Suply));
            }
        }
        private void Save()
        {
            try
            {
                FoodValidationRules.NameValidation(Name);
                FoodValidationRules.TypeValidation(Suply.Type);
                FoodValidationRules.FileExistsValidation(Suply.Path_to_Image);
                FoodValidationRules.PriceValidation(Suply.Price);

                var suplies = Data.LoadData<Models.Suply>("Data\\suplies.json");
                Suply.Name = Name;
                suplies.Add(this.Suply);
                Data.SaveData("Data\\suplies.json", suplies);
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
                FoodValidationRules.TypeValidation(Suply.Type);
                FoodValidationRules.FileExistsValidation(Suply.Path_to_Image);
                FoodValidationRules.PriceValidation(Suply.Price);

                var suplies = Data.LoadData<Models.Suply>(suplyFilePath);
                var existingApp = suplies.First(app => app.Name == this.Suply.Name);
                if (existingApp == null)
                {
                    throw new Exception("Застосунку не знайдено.");
                }

                existingApp.Name = Name;
                existingApp.Type = this.Suply.Type;
                existingApp.Price = this.Suply.Price;
                existingApp.Path_to_Image = this.Suply.Path_to_Image;

                Data.SaveData(suplyFilePath, suplies);

                _window.Close();
            } catch (Exception ex)
            {
                var win = new ErorWin();
                var viewModel = new ErrorViewModel(ex.Message, win);
                win.DataContext = viewModel;
                win.ShowDialog();
            }
        }
    }
}
