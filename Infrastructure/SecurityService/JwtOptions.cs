using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityService
{ 
    public class JwtOptions
    {
        public string Key = string.Empty;
        public string Issuer = string.Empty;
        public string Audience = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
    }

}
