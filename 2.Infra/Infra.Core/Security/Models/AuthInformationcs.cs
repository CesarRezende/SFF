namespace SFF.Infra.Core.Security.Models
{
    public class AuthInformation
    {
        public AuthInformation(
            string accessToken,
            int expiresIn,
            string refresToken,
            int refresTokenExpiresIn
            )
        {
            access_token = accessToken;
            expires_in = expiresIn;
            refresh_token = refresToken;
            refresh_token_expires_in = refresTokenExpiresIn;
        }

        public static AuthInformation CreateAuthInformation(
            string accessToken,
            int expiresIn,
            string refresToken,
            int refresTokenExpiresIn
            )
        {
            return new AuthInformation(
                    accessToken: accessToken,
                    expiresIn: expiresIn,
                    refresToken: refresToken,
                    refresTokenExpiresIn: refresTokenExpiresIn
                );
        }

        public string token_type { get; private set; } = "Bearer";
        public string access_token { get; private set; }
        public int expires_in { get; private set; }
        public string refresh_token { get; private set; }
        public int refresh_token_expires_in { get; private set; }

    }
}
