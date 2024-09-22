namespace BusinessLogic;

public class Business
{
    public string Rut { get; set; }
    public string Name { get; set; }
    public User Owner { get; set; }
    public Business(string s, string name, User owner)
    {
        throw new NotImplementedException();
    }
}
