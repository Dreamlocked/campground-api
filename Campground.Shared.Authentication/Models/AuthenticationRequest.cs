using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Authentication.Models
{
    public class AuthenticationRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
