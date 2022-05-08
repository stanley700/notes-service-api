using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NotesService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }
    }
}
