namespace Centraleo.Reservations.Models;

public class Reservation
{
    public int Id { get; }
    public int ResourceId { get; }
    public Person Client { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public ReservationStatus Status { get; private set; }

    public Reservation(int id, int resourceId, Person client, DateTime startDate, DateTime endDate, ReservationStatus status)
    {
        if (id <= 0) throw new ArgumentException("Id must be positive.");
        if (resourceId <= 0) throw new ArgumentException("ResourceId must be positive.");
        if (startDate.Date > endDate.Date) throw new ArgumentException("StartDate must be <= EndDate.");

        Id = id;
        ResourceId = resourceId;
        Client = client ?? throw new ArgumentNullException(nameof(client));
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        Status = status;
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Cancelled) return;
        Status = ReservationStatus.Cancelled;
    }
}
