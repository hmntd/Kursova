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
        public string Name { get; set; }
        public string Path_to_Application { get; set; }
        public string Path_to_Image { get; set; }
        public string Type { get; set; }
        public float Hours_Played {  get; set; } 
        public Application() { }
        public Application(string name, string path_to_app, string path_to_image, string type, float hoursPlayed)
        {
            Name = name;
            Path_to_Application = path_to_app;
            Path_to_Image = path_to_image;
            Type = type;
            Hours_Played = hoursPlayed;
        }
    }
}
