﻿using Microsoft.Win32;
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
        public ICommand Close_Click { get; }
        public ICommand FileDialog_Click { get; }
        public ICommand Save_Click { get; }
        public AddTimeViewModel(Models.Rate rate, Window window, string tw)
        {
            Rate = rate;
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
                RateValidationRules.NameValidation(Rate.Name);
                RateValidationRules.PriceValidation(Rate.Price);
                RateValidationRules.FileExistsValidation(Rate.Path_to_image);

                var rates = Data.LoadData<Models.Rate>(rateFilePath);
                rates.Add(this.Rate);
                Data.SaveData(rateFilePath, rates);
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
                RateValidationRules.NameValidation(Rate.Name);
                RateValidationRules.PriceValidation(Rate.Price);
                RateValidationRules.FileExistsValidation(Rate.Path_to_image);

                var rates = Data.LoadData<Models.Rate>(rateFilePath);

                var existingRate = rates.FirstOrDefault(r => r.Id == this.Rate.Id);
                if (existingRate == null)
                {
                    throw new Exception("Тарифу не знайдено");
                }

                existingRate.Name = this.Rate.Name;
                existingRate.Price = this.Rate.Price;
                existingRate.Path_to_image = this.Rate.Path_to_image;

                Data.SaveData(rateFilePath, rates);

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