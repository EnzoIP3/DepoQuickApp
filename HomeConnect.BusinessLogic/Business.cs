namespace BusinessLogic;

public class Business
{
    public string Rut { get; set; }
    public string Name { get; set; }
    public User Owner { get; set; }
    public Business(string rut, string name, User owner)
    {
        Rut = rut;
        Name = name;
        Owner = owner;
    }
}
