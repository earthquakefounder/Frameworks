using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain.Authentication
{
    public class AuthToken
    {
        [JsonProperty(PropertyName = "token_id")]
        public Guid TokenID { get; set; }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public DateTime Expiration { get; set; }
    }
}
