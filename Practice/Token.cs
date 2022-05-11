using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PersonManagement.MVC
{
    public static class Token
    {
        private const string Secret = "6de7bab2-53d8-4f00-b045-de7c57571345";
        private const string Issuer = "saba_maghlakelidze";
        private const string Audience = "saba_maghlakelidze";
        private static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        public static string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = SecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string GetLoginFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenStr = handler.ReadToken(token) as JwtSecurityToken;
                var login = tokenStr.Claims.First(claim => claim.Type == "nameid").Value;

                return login;
            }
            catch
            {
                return "";
            }
        }
    }
}
