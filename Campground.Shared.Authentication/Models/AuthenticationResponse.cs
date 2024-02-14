using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Authentication.Models
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }

    }
}
