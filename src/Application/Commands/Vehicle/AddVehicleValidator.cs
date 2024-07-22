using FluentValidation;

namespace Application.Commands.User
{
    internal class AddVehicleValidator : AbstractValidator<AddVehicleCommand>
    {
        public AddVehicleValidator()
        {
            RuleFor(x => x.Plate).NotEmpty().WithMessage("Placa é obrigatória");
        }

    }
}
