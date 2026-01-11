using System.Text;
using Centraleo.Reservations.Models;

namespace Centraleo.Reservations.Utils;

public static class ReservationFormatter
{
    public static string Format(Reservation reservation, Resource resource)
    {
        var sb = new StringBuilder();

        sb.AppendLine("══════════════════════════════════════════════════════════");
        sb.AppendLine("RÉCAPITULATIF DE RÉSERVATION");
        sb.AppendLine("══════════════════════════════════════════════════════════");

        sb.AppendLine("Ressource");
        sb.AppendLine("---------");
        sb.AppendLine($"Type : {resource.Type}");
        sb.AppendLine($"Nom  : {resource.Name}");

        if (resource.Responsible != null)
        {
            sb.AppendLine($"Responsable : {resource.Responsible.Name}");
            sb.AppendLine($"Contact     : {resource.Responsible.Email}");
        }
        else
        {
            sb.AppendLine("Responsable : (Aucun)");
            sb.AppendLine("Contact     : (Aucun)");
        }

        sb.AppendLine("Client");
        sb.AppendLine("------");
        sb.AppendLine($"Nom   : {reservation.Client.Name}");
        sb.AppendLine($"Email : {reservation.Client.Email}");

        sb.AppendLine("Réservation");
        sb.AppendLine("-----------");

        // si tu veux 1 seule date quand start=end, sinon période
        if (reservation.StartDate == reservation.EndDate)
            sb.AppendLine($"Date   : {reservation.StartDate:dd MMMM yyyy}");
        else
            sb.AppendLine($"Dates  : {reservation.StartDate:dd MMMM yyyy} → {reservation.EndDate:dd MMMM yyyy}");

        sb.AppendLine($"Statut : {reservation.Status}");

        sb.AppendLine("══════════════════════════════════════════════════════════");

        return sb.ToString();
    }
}
