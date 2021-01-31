namespace DeliveryWebApi.DTO
{
    public class Api1Request
    {
        public string ContactAddress { get; set; }

        public string WarehouseAddress { get; set; }

        public double[] CartonDimensions { get; set; }
    }
}