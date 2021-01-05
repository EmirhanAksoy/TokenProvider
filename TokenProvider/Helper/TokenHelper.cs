using ArcelikAuthProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArcelikAuthProvider.Helper
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(User user, IList<string> roles, IConfiguration configuration)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                userClaims.Add(roleClaim);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]));
            var jwt = new JwtSecurityToken(
                claims: userClaims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtOptions:ExpireMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static string GetUserIdFromExpiredToken(string jwtToken)
        {
            JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
            var userId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
