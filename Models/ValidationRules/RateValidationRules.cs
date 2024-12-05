using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    public class RateValidationRules
    {
        public static bool NameValidation(string name)
        {
            if (name == null)
            {
                throw new Exception("назва тарифу не може бути порожньою");
            }

            var rates = Data.LoadData<Rate>("Data\\rates.json");
            if (rates.Any(g => g.Name == name))
            {
                throw new Exception("цей тариф вже існує");
            }

            return true;
        }
        public static bool FileExistsValidation(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("такого файлу немає на машині");
            }

            return true;
        }
        public static bool PriceValidation(decimal price)
        {
            if (price < 0)
            {
                throw new Exception("ціна на тариф не може бути нижчою за 0");
            }

            return true;
        }
    }
}
