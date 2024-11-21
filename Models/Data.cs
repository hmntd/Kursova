using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static ReestrForm.Page1;

namespace ReestrForm
{
    public class Data
    {
        public static ObservableCollection<T> LoadData<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<ObservableCollection<T>>(json);
            }

            return new ObservableCollection<T>();
        }

        public static void SaveData<T>(string filePath, ObservableCollection<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
