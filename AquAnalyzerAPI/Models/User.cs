using System.ComponentModel.DataAnnotations;

namespace AquAnalyzerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(3, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
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