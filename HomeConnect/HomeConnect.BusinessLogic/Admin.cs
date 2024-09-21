using System.Text.RegularExpressions;

namespace BusinessLogic;

public class Admin
{
    private readonly string _email = string.Empty;
    public string Username { get; set; }
    public string Surname { get; set; }

    public string Email
    {
        get => _email;
        private init
        {
            if (value == string.Empty)
            {
                throw new Exception("Email is invalid.");
            }

            const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(value, emailPattern))
            {
                throw new Exception("Email format invalid.");
            }

            _email = value;
        }
    }

    public string Password { get; set; }

    public Admin(string username, string surname, string email, string password)
    {
        Username = username;
        Surname = surname;
        Email = email;
        Password = password;
    }
}
