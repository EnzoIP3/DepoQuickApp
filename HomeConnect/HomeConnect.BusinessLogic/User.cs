using System.ComponentModel.DataAnnotations;
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
            ValidateNotEmpty(value, nameof(Name));
            _name = value;
        }
    }

    public string Surname
    {
        get => _surname;
        private init
        {
            ValidateNotEmpty(value, nameof(Surname));
            _surname = value;
        }
    }

    [Key]
    public string Email
    {
        get => _email;
        private init
        {
            ValidateNotEmpty(value, nameof(Email));
            const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(value, emailPattern))
            {
                throw new ArgumentException("Email format invalid.");
            }

            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        private init
        {
            ValidateNotEmpty(value, nameof(Password));
            ValidatePassword(value);
            _password = value;
        }
    }

    public Role Role { get; set; } = new Role();
    public DateOnly CreatedAt { get; set; }

    public User()
    {
    }

    public User(string name, string surname, string email, string password, Role role)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        Role = role;
    }

    private static void ValidateNotEmpty(string value, string propertyName)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
        {
            throw new Exception($"{propertyName} cannot be blank.");
        }
    }

    private static void ValidatePassword(string password)
    {
        if (!ContainsCapitalLetter(password))
        {
            throw new Exception("Password must contain at least one capital letter.");
        }

        if (!ContainsDigit(password))
        {
            throw new Exception("Password must contain at least one digit.");
        }

        if (!ContainsSpecialCharacter(password))
        {
            throw new Exception("Password must contain at least one special character.");
        }

        if (password.Length < 8)
        {
            throw new Exception("Password must be at least 8 characters long.");
        }
    }

    private static bool ContainsCapitalLetter(string input)
    {
        const string capitalLetterPattern = @"[A-Z]";
        return Regex.IsMatch(input, capitalLetterPattern);
    }

    private static bool ContainsDigit(string input)
    {
        const string digitPattern = @"\d";
        return Regex.IsMatch(input, digitPattern);
    }

    private static bool ContainsSpecialCharacter(string input)
    {
        const string specialCharacterPattern = @"[\W_]";
        return Regex.IsMatch(input, specialCharacterPattern);
    }
}
