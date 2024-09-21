using System.Text.RegularExpressions;

namespace BusinessLogic;

public class Admin
{
    private readonly string _email = string.Empty;
    private readonly string _username = string.Empty;
    private readonly string _surname = string.Empty;
    private readonly string _password = string.Empty;

    public string Username
    {
        get => _username;
        private init
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Arguments cannot be blank.");
            }

            _username = value;
        }
    }

    public string Surname
    {
        get => _surname;
        private init
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Arguments cannot be blank.");
            }

            _surname = value;
        }
    }

    public string Email
    {
        get => _email;
        private init
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Arguments cannot be blank.");
            }

            const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(value, emailPattern))
            {
                throw new Exception("Email format invalid.");
            }

            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        private init
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Arguments cannot be blank.");
            }

            _password = value;
        }
    }

    public Admin(string username, string surname, string email, string password)
    {
        Username = username;
        Surname = surname;
        Email = email;
        Password = password;
    }
}
