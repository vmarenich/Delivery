using Microsoft.AspNetCore.Mvc;
using DeliveryWebApi.DTO;
using DeliveryWebApi.Services;

namespace DeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class Api2Controller : ControllerBase
    {
        public Api2Controller(IOffersAppService offersService)
        {
            OffersService = offersService;
        }

        private IOffersAppService OffersService { get; }

        [HttpGet]
        public ActionResult<Api2Result> Get(Api2Request request)
        {
            var result = OffersService.Calculate(request.Consignee, request.Consignor, request.Cartons);
            if (double.IsNaN(result))
            {
                return NotFound();
            }

            return new Api2Result { Amount = result };
        }
    }
}