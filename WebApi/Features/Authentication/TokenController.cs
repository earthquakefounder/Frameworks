using Entities.Contexts;
using Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebApi.Domain;
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
            ValidationResult<Guid, ChangePasswordResult> result = null;

            if (result = await userContext.AuthenticateAsync(request.UserName, request.Password))
            {
                var handler = new JwtSecurityTokenHandler();
                var descriptor = new SecurityTokenDescriptor
                {
                    Issuer = tokenOptions.Issuer,
                    Audience = tokenOptions.Audience,
                    SigningCredentials = tokenOptions.SigningCredentials,
                    Subject = await CreateIdentity(result.Value),
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

        private async Task<ClaimsIdentity> CreateIdentity(Guid user_id)
        {
            var user = await userContext.Entities.SingleOrDefaultAsync(x => x.ID == user_id);

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(Claims.UserID, user.ID.Value.ToString()),
                new Claim(Claims.UserName, user.UserName),
                new Claim(Claims.Name, user.Name)
            });

            return claims;
        }
    }
}
