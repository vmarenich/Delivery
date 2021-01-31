namespace DeliveryWebApi.DTO
{
    public class Api2Request
    {
        public string Consignee { get; set; }

        public string Consignor { get; set; }

        public double[] Cartons { get; set; }
    }
}