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
        private static int EXPIRATION_IN_MINUTES { get; set; }
        private static int REFRESH_TOKEN_EXPIRATION_IN_MINUTES { get; set; }
        private static byte[] SECRET_KEY { get; set; }
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            EXPIRATION_IN_MINUTES = Convert.ToInt32(_configuration[$"Security:TokenParameters:ExpirationInMinutes"]);
            EXPIRATION_IN_MINUTES = Convert.ToInt32(_configuration[$"Security:TokenParameters:RefreshTokenExpirationInMinutes"]);
            SECRET_KEY = Encoding.ASCII.GetBytes(_configuration[$"Security:TokenParameters:SecretKey"]);
        }

        public AuthInformation GenerateJWTToken(UserAuthInformation user, List<Claim> claims = null) 
        {
            var acessToken = GenerateAccessToken(user, claims);
            var refreshToken = GenerateRefreshToken();

            return AuthInformation.CreateAuthInformation(
                accessToken: acessToken,
                expiresIn: (EXPIRATION_IN_MINUTES * 60),
                refresToken: refreshToken,
                refresTokenExpiresIn: (REFRESH_TOKEN_EXPIRATION_IN_MINUTES * 60)
                );
        }

        private string GenerateAccessToken(UserAuthInformation user, List<Claim> claims = null)
        {
            dynamic token;
            var tokenHandler = new JwtSecurityTokenHandler();
            

            if (claims == null || !claims.Any())
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
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(EXPIRATION_IN_MINUTES),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SECRET_KEY), SecurityAlgorithms.HmacSha256Signature)
                };

                token = tokenHandler.CreateToken(tokenDescriptor);
            }
            else
            {

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(EXPIRATION_IN_MINUTES),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SECRET_KEY), SecurityAlgorithms.HmacSha256Signature)
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
