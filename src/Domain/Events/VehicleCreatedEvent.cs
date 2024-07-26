using Domain.Abstraction.Events;
using Domain.Entities;

namespace Domain.Events
{
    public class VehicleCreatedEvent : IEvent
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public DateTime Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public VehicleCreatedEvent(Vehicle vehicle)
        {
            Id = vehicle.Id.ToString();
            Identifier = vehicle.Identifier;
            Year = vehicle.Year;
            Model = vehicle.Model;
            Plate = vehicle.Plate;
        }
        public VehicleCreatedEvent()
        {
            Id = string.Empty;
            Identifier = string.Empty;
            Model = string.Empty;
            Plate = string.Empty;
        }
    }
}
