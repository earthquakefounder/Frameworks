using Entities.Contexts;
using Entities.Models.Identity;
using Infrastructure.Encryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Features.Accounts.Models;

namespace WebApi.Features.Accounts
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        IEncryptor encryptor;
        UserStorageContext userStorage;
        public AccountController(IEncryptor encryptor, UserStorageContext userStorage)
        {
            this.encryptor = encryptor;
            this.userStorage = userStorage;
        }

        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userStorage.Entities.Any(x => x.Email == user.EmailAddress))
                return BadRequest("The email address has already been registered");

            if (!string.IsNullOrWhiteSpace(user.UserName) && userStorage.Entities.Any(x => x.UserName == user.UserName.Trim()))
                return BadRequest("The username has already been taken");
            
            var entity = userStorage.Add(new AppUser()
            {
               Email = user.EmailAddress,
               Name = user.Name,
               UserName = string.IsNullOrWhiteSpace(user.UserName) ? null : user.UserName
            });

            await userStorage.UpdatePasswordAsync(entity, user.Password);

            await userStorage.SaveChangesAsync();

            return Ok();
        }

        [Route("me")]
        public async Task<IActionResult> ActiveUser()
        {
            return null;
        }
    }
}
