using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Identity
{
    public class AppUser
    {
        public Guid? ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
    }
}
