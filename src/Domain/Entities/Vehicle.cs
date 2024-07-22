using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Index(nameof(Plate), IsUnique = true)]
    public sealed class Vehicle : AggregateRoot
    {
        public string Identifier { get; set; }
        public DateTime Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public Vehicle()
        {
            Model = string.Empty;
            Plate = string.Empty;
            Year = new DateTime();
            Identifier = string.Empty;
        }

        public Vehicle(string identifier, DateTime year, string model, string plate)
        {
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
            AddEvent(new VehicleCreatedEvent(this));
        }
    }
}
