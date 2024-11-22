using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    public class RegisterValidationRules
    {
        public static bool PassswordValidate(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 8)
            {
                throw new Exception("Пароль має бути не менш ніж 8 символів довжиною");
            }

            if (!input.Any(char.IsDigit))
            {
                throw new Exception("Пароль має містити цифри");
            }

            return true;
        }
        public static bool UsernameValidate(string input)
        {
            if (input.Length < 3)
            {
                throw new Exception("Логін має бути довжиною не меншем 3 символів");
            }

            return true;
        }
        public static bool EmailValidate(string input)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(input);
                return true;
            }
            catch
            {
                throw new Exception("Некоректна електронна пошта");
            }
        }
        public static bool UserExistsValidate(User user, ObservableCollection<User> users)
        {
            if (users.Any(u => u.Username == user.Username))
            {
                throw new Exception("Вже є користувач з таким іменем");
            }

            if (users.Any(u => u.Email == user.Email))
            {
                throw new Exception("Вже є користувач з таким емейлом");
            }

            return true;
        }
    }
}
