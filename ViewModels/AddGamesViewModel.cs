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
                var apps = Data.LoadData<Models.Application>("Data\\applications.json");
                apps.Add(this.Application);
                Data.SaveData("Data\\applications.json", apps);
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
                GameValidationRules.NameValidation(this.Application.Name);
                GameValidationRules.TypeValidation(this.Application.Type);
                GameValidationRules.FileExistsValidation(this.Application.Path_to_Application);
                GameValidationRules.FileExistsValidation(this.Application.Path_to_Image);

                var apps = Data.LoadData<Models.Application>("Data\\applications.json");

                var existingApp = apps.FirstOrDefault(app => app.Id == this.Application.Id);
                if (existingApp == null)
                {
                    throw new Exception("Застосунку не знайдено.");
                }

                existingApp.Name = this.Application.Name;
                existingApp.Type = this.Application.Type;
                existingApp.Path_to_Application = this.Application.Path_to_Application;
                existingApp.Path_to_Image = this.Application.Path_to_Image;

                Data.SaveData("Data\\applications.json", apps);

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

    }
}
