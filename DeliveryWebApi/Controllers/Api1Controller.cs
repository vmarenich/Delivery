using Microsoft.AspNetCore.Mvc;
using DeliveryWebApi.DTO;
using DeliveryWebApi.Services;

namespace DeliveryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class Api1Controller : ControllerBase
    {
        public Api1Controller(IOffersAppService offersService)
        {
            OffersService = offersService;
        }

        private IOffersAppService OffersService { get; }

        [HttpGet]
        public ActionResult<Api1Result> Get(Api1Request request)
        {
            var result = OffersService.Calculate(request.ContactAddress, request.WarehouseAddress, request.CartonDimensions);
            if (double.IsNaN(result))
            {
                return NotFound();
            }

            return new Api1Result { Total = result };
        }
    }
}