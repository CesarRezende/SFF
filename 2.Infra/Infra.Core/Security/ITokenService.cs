using SFF.Infra.Core.Security.Models;
using System.Security.Claims;

namespace EaiBrasil.Kornerstone.KashApp.Infra.Security.Token
{
    public interface ITokenService
    {
        string GenerateToken(string securityStamp, UserAuthInformation user = null, List<Claim> claims = null);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
