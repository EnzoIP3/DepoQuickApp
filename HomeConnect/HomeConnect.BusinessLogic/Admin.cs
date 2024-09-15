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
