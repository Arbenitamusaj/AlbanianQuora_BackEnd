using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AlbanianQuora.Services
{
    public class TokenService
    {
        public static string GenerateToken(int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ALBANIAN_QUORA_SECRET_KEY_TOKEN_GENERATE");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, id.ToString())}),
               Expires = DateTime.UtcNow.AddDays(7),
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ALBANIAN_QUORA_SECRET_KEY_TOKEN_GENERATE");
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Log or handle token expiration specifically
                throw new SecurityTokenExpiredException("Token has expired.");
            }
            catch (SecurityTokenException ex)
            {
                // Log or handle generic token issues
                throw new SecurityTokenException("Token validation failed.", ex);
            }
        }

    }
}
