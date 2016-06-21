using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models.Identity;
using WebApi.Services.Interfaces;
using Entities.Contexts;

namespace WebApi.Services.Implementions
{
    public class Authentication : IAuthentication
    {
        private IStorageContext<AppUser> userStorage;

        public Authentication(IStorageContext<AppUser> userStorage)
        {
            this.userStorage = userStorage;
        }

        public bool Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string username, string password, out AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
