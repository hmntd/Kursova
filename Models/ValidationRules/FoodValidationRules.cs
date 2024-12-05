using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    public class FoodValidationRules
    {
        public static bool NameValidation(string name)
        {
            if (name == null)
            {
                throw new Exception("назва їжі не може бути порожньою");
            }

            var foods = Data.LoadData<Suply>("Data\\suplies.json");
            if (foods.Any(g => g.Name == name))
            {
                throw new Exception("ця їжа вже існує");
            }

            return true;
        }
        public static bool TypeValidation(string type)
        {
            if (type != "Drink" && type != "Food")
            {
                throw new Exception("некорректний тип застосунку");
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
                throw new Exception("ціна на їжу не може бути нижчою за 0");
            }

            return true;
        }
    }
}
