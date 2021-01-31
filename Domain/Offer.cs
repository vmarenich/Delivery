namespace WebApplication1.Domain
{
    public class Offer : EntityBase
    {
        public int SupplierId { get; set; }

        public string SourceAddress { get; set; }

        public string DestAddress { get; set; }

        public double Price { get; set; }
    }
}