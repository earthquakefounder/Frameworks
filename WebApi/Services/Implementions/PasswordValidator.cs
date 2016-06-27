using Entities.Contexts;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.Implementions
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private UserStorageContext _userContext;

        public PasswordValidator(UserStorageContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<CustomGrantValidationResult> ValidateAsync(string userName, string password, ValidatedTokenRequest request)
        {
            bool authenticated = await _userContext.AuthenticateAsync(userName, password);

            if (!authenticated)
                return new CustomGrantValidationResult("Invalid username or password");

            var user = await _userContext.Entities.SingleAsync(x => x.Email == userName);

            return new CustomGrantValidationResult(user.ID.ToString(), "password");
        }
    }
}
