using Microsoft.IdentityModel.Tokens;
using Progress_Finances_API.Data;
using Progress_Finances_API.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Progress_Finances_API.Services
{
    public class TokenAuth
    {
        public string GerarToken(Usuarios u)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration.ChavePrivada);
            var credencias = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GerarClaims(u),
                SigningCredentials = credencias,
                Expires = DateTime.UtcNow.AddDays(1),
            };

            var token = handler.CreateToken(tokenDescriptor);

            var stringToken = handler.WriteToken(token);

            return stringToken;
        }

        public static ClaimsIdentity GerarClaims(Usuarios u)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, u.Email));

            return ci;
        }
    }
}
