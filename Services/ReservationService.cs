using Centraleo.Reservations.Models;

namespace Centraleo.Reservations.Services;

public class ReservationService
{
    private readonly List<Reservation> _reservations = new();
    private int _nextId = 1;

    public IReadOnlyList<Reservation> GetAll() => _reservations;

    public Reservation? GetById(int id) => _reservations.FirstOrDefault(r => r.Id == id);

    public IReadOnlyList<Reservation> GetByResource(int resourceId) =>
        _reservations.Where(r => r.ResourceId == resourceId).ToList();

    public Reservation Create(int resourceId, Person client, DateTime start, DateTime end)
    {
        // règle : pas de chevauchement (sauf si annulée)
        if (HasConflict(resourceId, start, end))
            throw new InvalidOperationException("Conflict: this resource is already reserved for that period.");

        var reservation = new Reservation(
            id: _nextId++,
            resourceId: resourceId,
            client: client,
            startDate: start,
            endDate: end,
            status: ReservationStatus.Confirmed // tu peux mettre Pending si tu veux
        );

        _reservations.Add(reservation);
        return reservation;
    }

    public void Cancel(int reservationId)
    {
        var r = GetById(reservationId) ?? throw new InvalidOperationException("Reservation not found.");
        r.Cancel();
    }

    private bool HasConflict(int resourceId, DateTime start, DateTime end)
    {
        var s = start.Date;
        var e = end.Date;

        return _reservations.Any(r =>
            r.ResourceId == resourceId &&
            r.Status != ReservationStatus.Cancelled &&
            s <= r.EndDate && r.StartDate <= e  // chevauchement
        );
    }
}
