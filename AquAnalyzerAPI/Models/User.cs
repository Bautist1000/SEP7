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
    public User(int id, string username)
    {
        this.Id = id;
        this.Username = username;
    }
    public User(int id, string username, string password, string email, string role)
    {
        this.Id I = id;
        this.Username = username;
        this.Password = password;
        this.Email = email;
        this.Role = role;
    }
}