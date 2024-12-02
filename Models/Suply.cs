using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Suply
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Path_to_Image { get; set; }
        public int WasBought { get; set; }
        public Suply () { }
        public Suply(string id, string name, decimal price, int wasBought)
        {
            Id = id;
            Name = name;
            Price = price;
            WasBought = wasBought;
        }
    }
}
