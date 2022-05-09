using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesService.API.Models;
using NotesService.Core;
using NotesService.Core.Services;
using NotesService.Core.Services.IServices;
using System.Threading.Tasks;

namespace NotesService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(AddUserModel model)
        {
            var passowrd = CryptoService.GenerateHash(model.Password);
            var result = await _userService.CreateUser(new Core.Data.Entities.User(model.FirstName, model.LastName, model.Username, passowrd, true, model.RoleId));
            
            
            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }
    }
}
