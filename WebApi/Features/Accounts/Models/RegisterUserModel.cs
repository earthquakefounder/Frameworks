using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Features.Accounts.Models
{
    public class RegisterUserModel
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
