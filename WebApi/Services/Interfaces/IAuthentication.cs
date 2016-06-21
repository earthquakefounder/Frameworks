using Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    public interface IAuthentication
    {
        bool Authenticate(string username, string password);
        bool Authenticate(string username, string password, out AppUser user);
    }
}
