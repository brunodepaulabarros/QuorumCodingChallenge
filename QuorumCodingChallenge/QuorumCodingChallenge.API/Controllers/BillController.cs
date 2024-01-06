using Microsoft.AspNetCore.Mvc;
using QuorumCodingChallenge.Application.Services.BillServices;

namespace QuorumCodingChallenge.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        [Route("result")]
        [Produces("application/json")]
        public IActionResult GetResult()
        {
            var response = _billService.Result();
            return Ok(response);
        }
    }
}
