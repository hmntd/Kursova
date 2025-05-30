using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int Suply_Name { get; set; } // ID товару

        public int? Rate_name = null; // ID тарифу (не використовується поки що)

        public int Client_Name { get; set; } // ID користувача

        public int Count { get; set; }
        public bool Complited { get; set; }

        public Order() { }

        public Order(int id, int suply_id, int client_id, int count, bool complited)
        {
            Id = id;
            Suply_Name = suply_id;
            Client_Name = client_id;
            Count = count;
            Complited = complited;
        }
    }
}

