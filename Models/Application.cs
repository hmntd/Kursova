using ReestrForm.Models.ValidationRules;
using ReestrForm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public enum TypeApplication
    {
        App,
        Game
    }

    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path_to_Application { get; set; }
        public string Path_to_Image { get; set; }
        public string Type { get; set; }
        public float HoursPlayed {  get; set; } 
        public Application() { }
        public Application(Guid id, string name, string path_to_app, string path_to_img, string type, float hoursPlayed)
        {
            Id = id.ToString();
            Name = name;
            Path_to_Application = path_to_app;
            Path_to_Image = path_to_img;
            Type = type;
            HoursPlayed = hoursPlayed;
        }
    }
}
