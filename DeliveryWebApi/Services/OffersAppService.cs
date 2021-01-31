using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using DeliveryWebApi.Domain;

namespace DeliveryWebApi.Services
{
    public class OffersAppService : AppServiceBase, IOffersAppService
    {
        public OffersAppService(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            var suppliers = GetRepository<Supplier>();
            suppliers.Create(new Supplier { Name = "Supplier1" });
            suppliers.Create(new Supplier { Name = "Supplier2" });
            suppliers.Create(new Supplier { Name = "Supplier3" });
            suppliers.Create(new Supplier { Name = "Supplier4" });
            suppliers.Create(new Supplier { Name = "Supplier5" });
            suppliers.Create(new Supplier { Name = "Supplier6" });
            suppliers.Create(new Supplier { Name = "Supplier7" });
            suppliers.Create(new Supplier { Name = "Supplier8" });
            suppliers.Create(new Supplier { Name = "Supplier9" });
        }

        public List<Offer> GetAll()
        {
            return GetRepository<Offer>().All.ToList();
        }

        public Offer Get(int id)
        {
            return GetRepository<Offer>().Find(id);
        }

        public Offer AddOrUpdate(Offer offer)
        {
            var repository = GetRepository<Offer>();
            var entity = repository
                .All
                .FirstOrDefault(p =>
                    p.SupplierId == offer.SupplierId &&
                    string.Equals(p.SourceAddress, offer.SourceAddress, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(p.DestAddress, offer.DestAddress, StringComparison.CurrentCultureIgnoreCase));

            if (entity == null)
            {
                repository.Create(offer);
            }
            else
            {
                entity.SourceAddress = offer.SourceAddress;
                entity.DestAddress = offer.DestAddress;
                entity.Price = offer.Price;
                repository.Update(entity);
            }

            return entity;
        }

        public double Calculate(string source, string destination, double[] packages)
        {
            var entity = GetRepository<Offer>()
                .All
                .Where(p =>
                    string.Equals(p.SourceAddress, source, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(p.DestAddress, destination, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Price)
                .FirstOrDefault();

            return entity == null ? double.NaN : entity.Price * packages.Sum();
        }
    }
}
