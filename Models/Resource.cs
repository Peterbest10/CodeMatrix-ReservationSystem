namespace Centraleo.Reservations.Models;

public class Resource
{
    public int Id { get; }
    public ResourceType Type { get; }
    public string Name { get; }
    public Person? Responsible { get; }

    public Resource(int id, ResourceType type, string name, Person? responsible = null)
    {
        if (id <= 0) throw new ArgumentException("Id must be positive.");
        Id = id;
        Type = type;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required.") : name.Trim();
        Responsible = responsible;
    }

    public override string ToString() => $"#{Id} - {Type} - {Name}";
}
