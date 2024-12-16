using System;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public User()
        {

        }
        public User(int Id, string Username)
        {
            this.Id = Id;
            this.Username = Username;
        }
        public User(int Id, string Username, string Password, string Email, string Role)
        {
            this.Id = Id;
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;
            this.Role = Role;
        }
    }
}
