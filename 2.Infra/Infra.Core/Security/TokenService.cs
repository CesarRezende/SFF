using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SFF.Infra.Core.Security.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EaiBrasil.Kornerstone.KashApp.Infra.Security.Token.Implementations
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration { get; }

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string securityStamp, UserAuthInformation user = null, List<Claim> claims = null)
        {
            dynamic token;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration[$"Security:TokenParameters:SecretKey"]);
            string expirate = _configuration[$"Security:TokenParameters:ExpirationInMinutes"];

            if (claims == null)
            {

                var userData = JsonConvert.SerializeObject(
                                user,
                                Formatting.Indented,
                                new JsonSerializerSettings()
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.UserData, userData),
                        new Claim("SecurityStamp", securityStamp),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirate)),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                token = tokenHandler.CreateToken(tokenDescriptor);
            }
            else
            {

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirate)),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                token = tokenHandler.CreateToken(tokenDescriptor);
            }

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidatorParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration[$"Security:TokenParameters:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidatorParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
