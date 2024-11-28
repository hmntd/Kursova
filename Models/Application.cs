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
        public TypeApplication Type { get; set; }
        public Application() { }
        public Application(Guid id, string name, string path_to_app, string path_to_img, TypeApplication type)
        {
            Id = id.ToString();
            Name = name;
            Path_to_Application = path_to_app;
            Path_to_Image = path_to_img;
            Type = type;
        }
    }
}
