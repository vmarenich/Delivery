using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    public class Api3Controller : ControllerBase
    {
        public Api3Controller(IOffersAppService offersService)
        {
            OffersService = offersService;
        }

        private IOffersAppService OffersService { get; }

        [HttpGet]
        public ActionResult<Api3Result> Get(Api3Request request)
        {
            var result = OffersService.Calculate(request.Source, request.Destination, request.Packages);
            if (double.IsNaN(result))
            {
                return NotFound();
            }

            return new Api3Result { Quote = result };
        }
    }
}