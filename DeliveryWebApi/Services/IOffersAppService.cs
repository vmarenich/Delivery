using System.Collections.Generic;
using DeliveryWebApi.Domain;

namespace DeliveryWebApi.Services
{
    public interface IOffersAppService
    {
        List<Offer> GetAll();

        Offer Get(int id);

        Offer AddOrUpdate(Offer offer);

        double Calculate(string source, string destination, double[] packages);
    }
}