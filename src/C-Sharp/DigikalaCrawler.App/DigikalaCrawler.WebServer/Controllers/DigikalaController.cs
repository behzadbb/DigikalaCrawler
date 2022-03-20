using DigikalaCrawler.Data.Mongo;
using Microsoft.AspNetCore.Mvc;

namespace DigikalaCrawler.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigikalaController : ControllerBase
    {
        private readonly ILogger<DigikalaController> _logger;
        private readonly DigikalaMongo _digi;

        public DigikalaController(ILogger<DigikalaController> logger, DigikalaMongo digi)
        {
            _logger = logger;
            _digi = digi;
        }

        [HttpGet]
        public IEnumerable<string> GetFreePage()
        {
            return _digi.
        }
    }
}