using Campground.Shared.Authentication.Data;
using Campground.Shared.Authentication.Models;
using Campground.Shared.Authentication.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Authentication.Services
{
    public class UserAccountService(CampgroundContext dbContext)
    {
        protected readonly CampgroundContext _dbContext = dbContext;
        public async Task<UserDto?> AuthenticateUser(AuthenticationRequest request)
        {
            var userDto = await _dbContext.Users
                .Where(u => u.Username == request.Username)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Salt = u.Salt,
                    Password = u.Password,
                    Email = u.Email,
                    UrlPhoto = u.UrlPhoto
                })
                .SingleOrDefaultAsync();

            if(userDto != null && userDto.Password == Encript.GetSHA256Hash(request.Password + userDto.Salt)) return userDto;

            return null;
        }
    }
}
