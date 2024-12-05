using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models.ValidationRules
{
    public class GameValidationRules
    {
        public static bool NameValidation(string name)
        {
            if (name == null)
            {
                throw new Exception("назва гри не може бути порожньою");
            }

            var games = Data.LoadData<Application>("Data\\applications.json");
            if (games.Any(g => g.Name == name))
            {
                throw new Exception("ця гра вже існує");
            }

            return true;
        }
        public static bool TypeValidation(string type)
        {
            if (type != "Game" && type != "App")
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
    }
}
