using SFF.Infra.Core.Security.Models;
using System.Security.Claims;

namespace EaiBrasil.Kornerstone.KashApp.Infra.Security.Token
{
    public interface ITokenService
    {
        AuthInformation GenerateJWTToken(UserAuthInformation user, List<Claim> claims = null);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
