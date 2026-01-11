using Centraleo.Reservations.Models;
using Centraleo.Reservations.Services;
using Centraleo.Reservations.UI;

var resourceService = new ResourceService();
var reservationService = new ReservationService();

// Données de test (tu peux en ajouter)
resourceService.Add(new Resource(
    id: 1,
    type: ResourceType.Room,
    name: "Salle de réunion A",
    responsible: new Person("Marie Laurent", "marie.laurent@entreprise.com")
));

resourceService.Add(new Resource(
    id: 2,
    type: ResourceType.Equipment,
    name: "Projecteur HD",
    responsible: null
));

resourceService.Add(new Resource(
    id: 3,
    type: ResourceType.GuestRoom,
    name: "Chambre de passage 1",
    responsible: new Person("Jean Paul", "jean.paul@entreprise.com")
));

var menu = new Menu(resourceService, reservationService);
menu.Run();
