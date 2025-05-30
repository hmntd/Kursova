using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Is_Admin { get; set; }
        private float hours;
        private float total_hours;
        public float Balance { get; set; }
        private int? rate_name;
        public int? Rate_name
        {
            get => rate_name;
            set => rate_name = value;
        }
        public float Hours
        {
            get { return hours; }
            set
            {
                if (value < 0) throw new Exception("Not correct hours");
                hours = (float)Math.Round(value, 2);
            }
        }

        public float Total_Hours
        {
            get { return total_hours; }
            set
            {
                if (value < 0) throw new Exception("Not correct hours");
                total_hours = (float)Math.Round(value, 2);
            }
        }
        public User() { }
        public User(
            int id,
            string password,
            string username,
            string email,
            bool isAdmin,
            float hours,
            float totalHours,
            float balance,
            int? rate
            )
        {
            Id = id;
            Password = password;
            Username = username;
            Email = email;
            Is_Admin = isAdmin;
            Hours = hours;
            Total_Hours = totalHours;
            Balance = balance;
            rate_name = rate ?? null;
        }

    }
}
