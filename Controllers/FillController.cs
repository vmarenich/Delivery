using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillController : ControllerBase
    {
        public FillController(IOffersAppService offersService)
        {
            OffersService = offersService;
        }

        private IOffersAppService OffersService { get; }

        [HttpGet]
        public ActionResult<List<Offer>> Get() => OffersService.GetAll();

        [HttpGet("{id}", Name = nameof(GetOffer))]
        public ActionResult<Offer> GetOffer(int id)
        {
            var offer = OffersService.Get(id);
            if (offer == null)
            {
                return NotFound();
            }

            return offer;
        }

        [HttpPost]
        public ActionResult<Offer> Create(Offer offer)
        {
            OffersService.AddOrUpdate(offer);
            return CreatedAtRoute(nameof(GetOffer), new { id = offer.Id }, offer);
        }
    }
}
