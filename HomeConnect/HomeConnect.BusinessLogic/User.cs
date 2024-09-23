using System.Text.RegularExpressions;

namespace BusinessLogic;

public class User
{
    private readonly string _email = string.Empty;
    private readonly string _name = string.Empty;
    private readonly string _surname = string.Empty;
    private readonly string _password = string.Empty;

    public string Name
    {
        get => _name;
        private init
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Arguments cannot be blank.");
            }

            _name = value;
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

            const string capitalLetterPattern = @"[A-Z]";
            const string digitPattern = @"\d";
            const string specialCharacterPattern = @"[\W_]";
            const int minLength = 8;

            if (!Regex.IsMatch(value, capitalLetterPattern))
            {
                throw new Exception("Password must contain at least one capital letter.");
            }

            if (!Regex.IsMatch(value, digitPattern))
            {
                throw new Exception("Password must contain at least one digit.");
            }

            if (!Regex.IsMatch(value, specialCharacterPattern))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            if (value.Length < minLength)
            {
                throw new Exception("Password must be at least 8 characters long.");
            }

            _password = value;
        }
    }

    public Role Role { get; private set; }
    public DateOnly CreatedAt { get; private set; }

    public User(string name, string surname, string email, string password, string role)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);

        EnsureRoleIsValid(role);
        Role = Enum.Parse<Role>(role);
    }

    private static void EnsureRoleIsValid(string role)
    {
        if (string.IsNullOrEmpty(role) || string.IsNullOrWhiteSpace(role))
        {
            throw new Exception("Arguments cannot be blank.");
        }
    }
}
