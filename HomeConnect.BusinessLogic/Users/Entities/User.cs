using System.Text.RegularExpressions;
using BusinessLogic.Roles.Entities;

namespace BusinessLogic.Users.Entities;

public class User
{
    private readonly string _email = string.Empty;
    private readonly string _name = string.Empty;
    private readonly string _password = string.Empty;

    private readonly string? _profilePicture;
    private readonly string _surname = string.Empty;

    public User()
    {
    }

    public User(string name, string surname, string email, string password, Role role, string? profilePicture = null)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        Roles = [role];
        ProfilePicture = profilePicture;
    }

    public Guid Id { get; init; } = Guid.NewGuid();

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

    public string Email
    {
        get => _email;
        init
        {
            ValidateNotEmpty(value, nameof(Email));
            ValidateEmailFormat(value);
            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        init
        {
            ValidateNotEmpty(value, nameof(Password));
            ValidatePassword(value);
            _password = value;
        }
    }

    public List<Role> Roles { get; set; } = [];
    public DateOnly CreatedAt { get; set; }

    public string? ProfilePicture
    {
        get => _profilePicture;
        private init
        {
            if (value != null)
            {
                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    throw new ArgumentException("Profile picture must be a valid URL.");
                }
            }

            _profilePicture = value;
        }
    }

    private static void ValidateNotEmpty(string value, string propertyName)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{propertyName} cannot be blank.");
        }
    }

    private static void ValidateEmailFormat(string email)
    {
        const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            throw new ArgumentException("Email format invalid.");
        }
    }

    private static void ValidatePassword(string password)
    {
        EnsureContainsCapitalLetter(password);
        EnsureContainsDigit(password);
        EnsureContainsSpecialCharacter(password);
        EnsurePasswordLength(password);
    }

    private static void EnsureContainsCapitalLetter(string input)
    {
        const string capitalLetterPattern = @"[A-Z]";
        if (!Regex.IsMatch(input, capitalLetterPattern))
        {
            throw new ArgumentException("Password must contain at least one capital letter.");
        }
    }

    private static void EnsureContainsDigit(string input)
    {
        const string digitPattern = @"\d";
        if (!Regex.IsMatch(input, digitPattern))
        {
            throw new ArgumentException("Password must contain at least one digit.");
        }
    }

    private static void EnsureContainsSpecialCharacter(string input)
    {
        const string specialCharacterPattern = @"[\W_]";
        if (!Regex.IsMatch(input, specialCharacterPattern))
        {
            throw new ArgumentException("Password must contain at least one special character.");
        }
    }

    private static void EnsurePasswordLength(string input)
    {
        if (input.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.");
        }
    }

    public bool HasPermission(string permission)
    {
        return Roles.Any(role => role.HasPermission(permission));
    }

    public void AddRole(Role role)
    {
        EnsureRoleIsNotAdded(role);
        Roles.Add(role);
    }

    private void EnsureRoleIsNotAdded(Role role)
    {
        if (Roles.Any(r => r.Name == role.Name))
        {
            throw new InvalidOperationException("User already has this role.");
        }
    }

    public List<SystemPermission> GetPermissions()
    {
        return [];
    }
}
