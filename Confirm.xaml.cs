using ReestrForm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReestrForm
{
    /// <summary>
    /// Логика взаимодействия для Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {
        public Confirm()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (DataContext is ConfirmViewModel viewModel)
                {
                    viewModel.RequestClose += () =>
                    {
                        this.DialogResult = viewModel.Result;
                        this.Close();
                    };
                }
            };
        }
    }
}
