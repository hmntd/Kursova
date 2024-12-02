using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Rate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public decimal Price { get; set; }
        public string Path_to_image { get; set; }
        public int WasBought { get; set; }
        public Rate () { }
    }
}
