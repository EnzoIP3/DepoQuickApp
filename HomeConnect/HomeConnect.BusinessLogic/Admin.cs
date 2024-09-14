namespace BusinessLogic;

public class Admin
{
    public string Username { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Admin(string username, string surname, string email, string password)
    {
        Username = username;
        Surname = surname;
        Email = email;
        Password = password;
    }
}
