using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Authentication.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Salt { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? UrlPhoto { get; set; }
    }
}
