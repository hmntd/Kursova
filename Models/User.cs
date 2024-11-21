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
        public User() { }
        public User(Guid id, string password, string username, string email, bool isAdmin)
        {
            Id = id.ToString();
            Password = password;
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
        }
    }
}
