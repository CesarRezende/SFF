using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel;

namespace SFF.Domain.Administration.Core.Aggregates.SessionAggegate
{
    public class RefreshToken : ValueObject<RefreshToken>
    {
        public string Token { get; private set; }
        public DateTime ExpireTime { get; private set; }
        public bool IsExpired { get { return ExpireTime < DateTime.Now; } }

        public RefreshToken(string token, DateTime expireTime)
        {
            Token = token;
            ExpireTime = expireTime;
        }

        protected override bool EqualsCore(RefreshToken other)
        {
            return Token == other.Token && ExpireTime == other.ExpireTime;
        }

        protected override int GetHashCodeCore()
        {
            return Token.GetHashCode() + ExpireTime.GetHashCode();
        }


        public static RefreshToken CreateRefreshToken(string token, DateTime expireTime)
        {

            var newToken = new RefreshToken(token, expireTime);
            newToken.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(token, "RefreshToken.token", "Token não pode ser nulo ou vazio")
               .IsGreaterThan(expireTime, DateTime.Now, "refreshToken.expiredTime", "Data de expiração não posso ser menor do que a data atual"));
           
            return newToken;
        }

        public void Update(string token, DateTime expireTime)
        {
            Token = token;
            ExpireTime = expireTime;

            AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(token, "RefreshToken.token", "Token não pode ser nulo ou vazio")
               .IsGreaterThan(expireTime, DateTime.Now, "refreshToken.expiredTime", "Data de expiração não posso ser menor do que a data atual"));

        }
    }
}
