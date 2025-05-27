using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Suply
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Type { get; set; }
        public string Path_to_Image { get; set; }
        public int Bought_count { get; set; }
        public Suply () { }
        public Suply(string name, float price, int bought_count)
        {
            Name = name;
            Price = price;
            Bought_count = bought_count;
        }
    }
}
