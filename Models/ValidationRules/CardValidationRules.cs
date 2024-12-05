using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    public class CardValidationRules
    {
        public static bool CardNumberValidate(long number)
        {
            CreditCardAttribute creditCardAttribute = new CreditCardAttribute();
            if (!creditCardAttribute.IsValid(number.ToString()))
            {
                throw new ValidationException("Номер картки недійсний.");
            }

            return true;
        }
        public static bool NameOwnerValidate(string name)
        {
            string pattern = @"^[a-zA-Zа-яА-Яа-яА-ЯёЁ' -]+$";

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Ім'я власника не може бути пустим.");
            }

            if (!Regex.IsMatch(name, pattern))
            {
                throw new ValidationException("Ім'я власника може містити тільки літери, без цифр і пробілів.");
            }

            return true;
        }

        public static bool CvvValidation(int cvv)
        {
            if (string.IsNullOrEmpty(cvv.ToString()) || !cvv.ToString().All(char.IsDigit) || cvv.ToString().Length != 3)
            {
                throw new ValidationException("Невірний формат CVV-коду. Він повинен містити 3 цифри.");
            }

            return true;
        }
        public static bool ExpiryDateValidate(string expiryDate)
        {
            if (string.IsNullOrEmpty(expiryDate))
            {
                throw new ValidationException("Дата дії не може бути порожньою.");
            }

            if (expiryDate.Length == 5 && expiryDate[2] == '/')
            {
                string monthPart = expiryDate.Substring(0, 2);
                string yearPart = expiryDate.Substring(3, 2);

                if (int.TryParse(monthPart, out int month) && int.TryParse(yearPart, out int year))
                {
                    if (month < 1 || month > 12)
                    {
                        throw new ValidationException("Невірний місяць у даті дії.");
                    }

                    int fullYear = 2000 + year;

                    DateTime currentDate = DateTime.Now;

                    if (fullYear < currentDate.Year || (fullYear == currentDate.Year && month < currentDate.Month))
                    {
                        throw new ValidationException("Кредитна картка прострочена.");
                    }

                    return true;
                }
            }

            throw new ValidationException("Невірний формат дати дії."); ;
        }
        public static bool SumValidate(int sum)
        {
            if (sum < 0)
            {
                throw new Exception("Сума поповнення не може бути меньше 0");
            }

            return true;
        }
    }
}
