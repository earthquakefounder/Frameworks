using Entities.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Domain.Authentication;
using WebApi.Features.Authentication.Models;

namespace WebApi.Features.Authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class TokenController : BaseController
    {
        private readonly AuthTokenOptions tokenOptions;
        private readonly UserStorageContext userContext;
        public TokenController(AuthTokenOptions tokenOptions, UserStorageContext context)
        {
            this.tokenOptions = tokenOptions;
            userContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            if (await userContext.AuthenticateAsync(request.UserName, request.Password))
            {
                var handler = new JwtSecurityTokenHandler();
                var descriptor = new SecurityTokenDescriptor
                {
                    Issuer = tokenOptions.Issuer,
                    Audience = tokenOptions.Audience,
                    SigningCredentials = tokenOptions.SigningCredentials,
                    Subject = new ClaimsIdentity(),
                    Expires = DateTime.UtcNow.AddMinutes(120)
                };

                var token = handler.CreateToken(descriptor);

                return Ok(new AuthToken()
                {
                    TokenID = Guid.NewGuid(),
                    AccessToken = handler.WriteToken(token),
                    Expiration = descriptor.Expires.Value
                });
            }

            return BadRequest("Invalid Username or Password");
        }
    }
}
