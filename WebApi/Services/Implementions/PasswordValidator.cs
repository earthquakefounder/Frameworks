using Entities.Contexts;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.Implementions
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        public PasswordValidator(UserStorageContext userContext) { }
        public Task<CustomGrantValidationResult> ValidateAsync(string userName, string password, ValidatedTokenRequest request)
        {
            return null;
        }
    }
}
