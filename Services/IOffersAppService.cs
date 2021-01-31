using System.Collections.Generic;
using WebApplication1.Domain;

namespace WebApplication1.Services
{
    public interface IOffersAppService
    {
        List<Offer> GetAll();

        Offer Get(int id);

        Offer AddOrUpdate(Offer offer);

        double Calculate(string source, string destination, double[] packages);
    }
}