using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrForm.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        private float hours;
        private float totalHours;
        public float Hours
        {
            get { return hours; }
            set {
                if (value < 0) { throw new Exception("Not correct hours"); }
                hours = value;
            }
        }
        public float TotalHours
        {
            get { return totalHours; }
            set
            {
                if (value < 0) { throw new Exception("Not correct hours"); }
                totalHours = value;
            }
        }
        public User() { }
        public User(Guid id, string password, string username, string email, bool isAdmin, float hours, float totalHours)
        {
            Id = id.ToString();
            Password = password;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            Hours = hours;
            TotalHours = totalHours;
        }
    }
}
