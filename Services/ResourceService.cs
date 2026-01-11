using Centraleo.Reservations.Models;

namespace Centraleo.Reservations.Services;

public class ResourceService
{
    private readonly List<Resource> _resources = new();

    public IReadOnlyList<Resource> GetAll() => _resources;

    public Resource? GetById(int id) => _resources.FirstOrDefault(r => r.Id == id);

    public void Add(Resource resource)
    {
        if (GetById(resource.Id) != null)
            throw new InvalidOperationException("A resource with the same Id already exists.");

        _resources.Add(resource);
    }
}
