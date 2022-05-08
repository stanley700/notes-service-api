using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NotesService.API.Controllers
{
    public class BaseController : Controller
    {
        public string UsernameFromToken
        {
            get
            {
                if(User == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                return User.Claims.Where(claim => claim.Type == ClaimTypes.Email).Select(claim => claim.Value).SingleOrDefault();
            }
        }

        public string FullnameFromToken
        {
            get
            {
                if (User == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                return User.Claims.Where(claim => claim.Type == ClaimTypes.Name).Select(claim => claim.Value).SingleOrDefault();
            }
        }

        public string UserIdFromToken
        {
            get
            {
                if (User == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                return User.Claims.Where(claim => claim.Type == JwtRegisteredClaimNames.Jti).Select(claim => claim.Value).SingleOrDefault();
            }
        }
    }
}
