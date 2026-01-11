namespace Centraleo.Reservations.Models;

public class Person
{
    public string Name { get; }
    public string Email { get; }

    public Person(string name, string email)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? throw new ArgumentException("Email is required.") : email.Trim();
    }

    public override string ToString() => $"{Name} ({Email})";
}
