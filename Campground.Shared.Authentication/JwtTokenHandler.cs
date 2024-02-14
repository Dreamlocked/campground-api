using Campground.Shared.Authentication.Models;
using Campground.Shared.Authentication.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Authentication
{
    public class JwtTokenHandler(UserAccountService userAccountService)
    {
        private readonly UserAccountService _userAccountService = userAccountService;
        public const string JWT_SECURITY_KEY = "Bl8xp5CEFM22b1XvoXR6j04MSOk1sMRFKC62HX+5lFg=";
        private const int JWT_TOKEN_VALIDITY_MINS = 60;

        public async Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if(string.IsNullOrWhiteSpace(authenticationRequest.Username) || string.IsNullOrWhiteSpace(authenticationRequest.Password)) return null;

            var userAccount = await _userAccountService.AuthenticateUser(authenticationRequest);
            if (userAccount == null) return null;

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, userAccount.FirstName + userAccount.LastName),
                new("Id", userAccount.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, userAccount.Email)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                Username = userAccount.Username,
                ExpiresIn = (int) tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
