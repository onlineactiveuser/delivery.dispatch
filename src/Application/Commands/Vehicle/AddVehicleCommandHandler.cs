using Application.Abstraction.Messaging;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Commands.User
{
    public class AddVehicleCommandHandler : ICommandHandler<AddVehicleCommand, bool>
    {
        private readonly IUnitOfWorkService _unitOfWork;
        private readonly ILogger<AddVehicleCommandHandler> _logger;
        public AddVehicleCommandHandler(
            IUnitOfWorkService unitOfWork,
            ILogger<AddVehicleCommandHandler> logger
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(AddVehicleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Adding vehicle {0}", JsonSerializer.Serialize(command));

                //Caso commando nao for válido,
                //podemos criar uma arquitetura de response genérico para obter todos os erros do validationresult
                if (!command.IsValid()) return false;

                var existVehicle = await _unitOfWork.Vehicles.ExistVehicleByPlate(command.Plate);

                if (existVehicle) return false;

                Vehicle vehicle = new Vehicle(command.Identifier, command.Year, command.Model, command.Plate);
                await _unitOfWork.Vehicles.Add(vehicle);
                await _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} StackTrace: {1}", ex.Message,ex.StackTrace);
                throw;
            }

        }
    }
}
