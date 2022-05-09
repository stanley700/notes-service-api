using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesService.API.Models;
using NotesService.Core;
using NotesService.Core.Services;
using NotesService.Core.Services.IServices;
using System;
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
            var result = await _userService.CreateUser(new Core.Data.Entities.User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                IsActive = true,
                LastName = model.LastName,
                Password = passowrd,
                RoleId = model.RoleId,
                Username = model.Username
            });            
            
            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }
    }
}
