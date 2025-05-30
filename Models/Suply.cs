using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Suply
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Type { get; set; }
        public string Path_to_Image { get; set; }
        public int Bought_count { get; set; }
        public Suply () { }
        public Suply(int id, string name, float price, int bought_count)
        {
            Id = id;
            Name = name;
            Price = price;
            Bought_count = bought_count;
        }
    }
}
