using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Rate
    {
        public string Name { get; set; }
        public int Hours { get; set; }
        public float Price { get; set; }
        public string Path_to_image { get; set; }
        public int Bought_count { get; set; }
        public Rate () { }
    }
}
