using Application.Abstraction.Messaging;
using Application.Contracts.Requests.Vehicle;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.User
{
    public class AddVehicleCommand : Command<bool>
    {
        public string Identifier { get; private set; }
        public DateTime Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public AddVehicleCommand(AddVehicleRequest addVehicleRequest)
        {
            Identifier = addVehicleRequest.Identifier;
            Year = addVehicleRequest.Year;
            Model = addVehicleRequest.Model;
            Plate = addVehicleRequest.Plate;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddVehicleValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
