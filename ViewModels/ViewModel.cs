using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReestrForm.ViewModels
{
    abstract public class ViewModel: INotifyPropertyChanged
    {
        public const string userFilePath = "users";
        public const string applicationFilePath = "applications";
        public const string rateFilePath = "rates";
        public const string suplyFilePath = "suplies";
        public const string orderFilePath = "orders";
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void Minimize_Window(Window window)
        {
            if (window != null)
                window.WindowState = WindowState.Minimized;
        }
        protected static void Close_Window(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        protected static void Toogle_Window(Window window)
        {
            if (window != null)
            {
                if (window.WindowState == WindowState.Maximized && window.WindowStyle == WindowStyle.None)
                {
                    window.WindowStyle = WindowStyle.None;
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowStyle = WindowStyle.None;
                    window.WindowState = WindowState.Maximized;
                }
            }
        }
        protected static void Tg_Link()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://t.me/Hhmntd",
                UseShellExecute = true
            });
        }
        protected static void Inst_Link()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.instagram.com/qqbiscuit/",
                UseShellExecute = true
            });
        }
        protected static void Discord_Link()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/rXMHgFxwG2",
                UseShellExecute = true
            });
        }
    }
}
