using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> logger;

        public MainController(ILogger<MainController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            const string message = "Hello world";

            logger.Log(LogLevel.Information, "Just logging some execution here.");

            return await Task.FromResult(message)
                .ConfigureAwait(false);
        }
    }
}
