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
            expires_in = expiresIn.ToString();
            refresh_token = refresToken;
            refresh_expires_in = refresTokenExpiresIn.ToString();
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
        public string expires_in { get; private set; }
        public string refresh_token { get; private set; }
        public string refresh_expires_in { get; private set; }

    }
}
