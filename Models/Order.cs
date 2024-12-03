using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string Suply_Name { get; set; }
        public string Client_Name { get; set; }
        public int Count { get; set; }
        public Order() { }
        public Order(string id, string suply_name, string client_name, int count)
        {
            Id = id;
            Suply_Name = suply_name;
            Client_Name = client_name;
            Count = count;
        }
    }
}
