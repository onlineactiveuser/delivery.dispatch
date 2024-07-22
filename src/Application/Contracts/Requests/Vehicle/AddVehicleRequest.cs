namespace Application.Contracts.Requests.Vehicle
{
    public class AddVehicleRequest
    {
        public string Identifier { get; set; }
        public DateTime Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public AddVehicleRequest()
        {
            Model = string.Empty;
            Plate = string.Empty;
            Year = new DateTime();
            Identifier = string.Empty;
        }
    }
}
