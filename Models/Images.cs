using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public enum TypeImages
    {
        App,
        Rate,
        Suply
    }
    public class Images
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public float Type { get; set; }
    }
}
