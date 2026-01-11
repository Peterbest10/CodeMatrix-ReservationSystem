using Centraleo.Reservations.Models;
using Centraleo.Reservations.Services;
using Centraleo.Reservations.Utils;

namespace Centraleo.Reservations.UI;

public class Menu
{
    private readonly ResourceService _resourceService;
    private readonly ReservationService _reservationService;

    public Menu(ResourceService resourceService, ReservationService reservationService)
    {
        _resourceService = resourceService;
        _reservationService = reservationService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CENTRALEO — Gestion de Réservations ===");
            Console.WriteLine("1) Ressources");
            Console.WriteLine("2) Réservations");
            Console.WriteLine("3) Récapitulatif global");
            Console.WriteLine("0) Quitter");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": ResourcesMenu(); break;
                case "2": ReservationsMenu(); break;
                case "3": GlobalSummary(); break;
                case "0": return;
                default: Pause("Choix invalide."); break;
            }
        }
    }

    private void ResourcesMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Ressources ===");
            Console.WriteLine("1) Lister");
            Console.WriteLine("2) Ajouter");
            Console.WriteLine("0) Retour");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    foreach (var r in _resourceService.GetAll())
                        Console.WriteLine(r);
                    Pause();
                    break;

                case "2":
                    AddResourceFlow();
                    break;

                case "0":
                    return;

                default:
                    Pause("Choix invalide.");
                    break;
            }
        }
    }

    private void AddResourceFlow()
    {
        Console.Clear();
        Console.WriteLine("=== Ajouter une ressource ===");

        int id = ReadInt("Id (ex: 1): ");

        Console.WriteLine("Type: 1=Room, 2=GuestRoom, 3=Equipment");
        int t = ReadInt("Type: ");
        var type = (ResourceType)t;

        string name = ReadText("Nom: ");

        Console.Write("A un responsable ? (y/n): ");
        var hasResp = (Console.ReadLine()?.Trim().ToLower() == "y");

        Person? responsible = null;
        if (hasResp)
        {
            var rn = ReadText("Nom responsable: ");
            var re = ReadText("Email responsable: ");
            responsible = new Person(rn, re);
        }

        try
        {
            _resourceService.Add(new Resource(id, type, name, responsible));
            Pause("Ressource ajoutée ✅");
        }
        catch (Exception ex)
        {
            Pause($"Erreur: {ex.Message}");
        }
    }

    private void ReservationsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Réservations ===");
            Console.WriteLine("1) Créer");
            Console.WriteLine("2) Consulter (par ID)");
            Console.WriteLine("3) Annuler (par ID)");
            Console.WriteLine("4) Lister toutes");
            Console.WriteLine("0) Retour");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case "1": CreateReservationFlow(); break;
                case "2": ViewReservationFlow(); break;
                case "3": CancelReservationFlow(); break;
                case "4": ListReservationsFlow(); break;
                case "0": return;
                default: Pause("Choix invalide."); break;
            }
        }
    }

    private void CreateReservationFlow()
    {
        Console.Clear();
        Console.WriteLine("=== Créer une réservation ===");

        int resourceId = ReadInt("Id ressource: ");
        var resource = _resourceService.GetById(resourceId);
        if (resource == null)
        {
            Pause("Ressource introuvable.");
            return;
        }

        string cn = ReadText("Nom client: ");
        string ce = ReadText("Email client: ");
        var client = new Person(cn, ce);

        DateTime start = ReadDate("Date début (YYYY-MM-DD): ");
        DateTime end = ReadDate("Date fin   (YYYY-MM-DD): ");

        try
        {
            var reservation = _reservationService.Create(resourceId, client, start, end);
            Console.Clear();
            Console.WriteLine(ReservationFormatter.Format(reservation, resource));
            Pause("Réservation créée ✅");
        }
        catch (Exception ex)
        {
            Pause($"Erreur: {ex.Message}");
        }
    }

    private void ViewReservationFlow()
    {
        Console.Clear();
        int id = ReadInt("Id réservation: ");

        var r = _reservationService.GetById(id);
        if (r == null)
        {
            Pause("Réservation introuvable.");
            return;
        }

        var resource = _resourceService.GetById(r.ResourceId);
        if (resource == null)
        {
            Pause("Ressource liée introuvable (données incohérentes).");
            return;
        }

        Console.Clear();
        Console.WriteLine(ReservationFormatter.Format(r, resource));
        Pause();
    }

    private void CancelReservationFlow()
    {
        Console.Clear();
        int id = ReadInt("Id réservation à annuler: ");

        try
        {
            _reservationService.Cancel(id);
            Pause("Réservation annulée ✅");
        }
        catch (Exception ex)
        {
            Pause($"Erreur: {ex.Message}");
        }
    }

    private void ListReservationsFlow()
    {
        Console.Clear();
        var all = _reservationService.GetAll();

        if (all.Count == 0)
        {
            Pause("Aucune réservation.");
            return;
        }

        foreach (var r in all.OrderBy(x => x.StartDate))
        {
            Console.WriteLine($"#{r.Id} | Ressource:{r.ResourceId} | {r.StartDate:yyyy-MM-dd}→{r.EndDate:yyyy-MM-dd} | {r.Status} | {r.Client.Name}");
        }
        Pause();
    }

    private void GlobalSummary()
    {
        Console.Clear();
        Console.WriteLine("=== Récapitulatif global ===");

        foreach (var res in _resourceService.GetAll())
        {
            Console.WriteLine();
            Console.WriteLine($"--- {res} ---");

            var reservations = _reservationService
                .GetByResource(res.Id)
                .Where(r => r.Status != ReservationStatus.Cancelled)
                .OrderBy(r => r.StartDate)
                .ToList();

            if (reservations.Count == 0)
            {
                Console.WriteLine("Aucune réservation active.");
                continue;
            }

            foreach (var r in reservations)
                Console.WriteLine($"#{r.Id} | {r.StartDate:yyyy-MM-dd}→{r.EndDate:yyyy-MM-dd} | {r.Status} | {r.Client.Name}");
        }

        Pause();
    }

    // ===== helpers console =====

    private static void Pause(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
            Console.WriteLine(message);

        Console.WriteLine("\nAppuie sur Entrée pour continuer...");
        Console.ReadLine();
    }

    private static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (int.TryParse(Console.ReadLine(), out int v) && v > 0) return v;
            Console.WriteLine("Valeur invalide, réessaie.");
        }
    }

    private static string ReadText(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(s)) return s;
            Console.WriteLine("Champ obligatoire.");
        }
    }

    private static DateTime ReadDate(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine()?.Trim();
            if (DateTime.TryParse(s, out var d)) return d.Date;
            Console.WriteLine("Date invalide. Format conseillé: YYYY-MM-DD");
        }
    }
}
