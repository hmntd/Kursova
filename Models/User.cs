using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class User
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Is_Admin { get; set; }
        private float hours;
        private float total_hours;
        public float Balance { get; set; }
        private string rate_name;
        public string Rate_name
        {
            get
            {
                

                return rate_name;
            }
            set
            {
                rate_name = value;
            }
        }
        public float Hours
        {
            get { return hours; }
            set {
                if (value < 0) { throw new Exception("Not correct hours"); }
                hours = value;
            }
        }
        public float Total_Hours
        {
            get { return total_hours; }
            set
            {
                if (value < 0) { throw new Exception("Not correct hours"); }
                total_hours = value;
            }
        }
        public User() { }
        public User(
            string password,
            string username,
            string email,
            bool isAdmin,
            float hours,
            float totalHours,
            float balance,
            string? rate
            )
        {
            Password = password;
            Username = username;
            Email = email;
            Is_Admin = isAdmin;
            Hours = hours;
            Total_Hours = totalHours;
            Balance = balance;
            rate_name = rate;
        }
    }
}
