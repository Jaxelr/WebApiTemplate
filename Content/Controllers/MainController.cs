using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiTemplate.Repositories;

namespace WebApiTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> logger;
        private readonly IRepository repository;

        public MainController(ILogger<MainController> logger, IRepository repository) =>
            (this.logger, this.repository) = (logger, repository);

        [HttpGet]
        public async Task<string> Get()
        {
            const string message = "Hello world";

            logger.Log(LogLevel.Information, "Just logging some execution here.");

            return await Task.FromResult(message)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<string> Get(string name)
        {
            logger.Log(LogLevel.Information, "Just logging some execution here.");

            return await Task.FromResult(repository.MakeHello(name))
                .ConfigureAwait(false);
        }
    }
}
