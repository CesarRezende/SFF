using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel.Base;

namespace SFF.Domain.Administration.Core.Aggregates.SessionAggegate
{
    public class Session : AggregateRoot<Guid>
    {

        public IPAddress IPAddress { get; private set; }
        public DateTime ExpireTime { get; private set; }

        public RefreshToken RefreshToken { get; private set; }
        public bool IsExpired { get { return ExpireTime < DateTime.Now; } }

        public Session(
            Guid id,
            DateTime expireTime,
            RefreshToken refreshToken,
            DateTime createdTime,
            DateTime? updatedTime)
            : base(id, createdTime, updatedTime)
        {
            ExpireTime = expireTime;
            RefreshToken = refreshToken;
        }


        public static Session CreateSession(
            Guid id,
            DateTime expireTime,
            string refreshToken,
            DateTime refreshTokenExpireTime)
        {

            var newToken =  RefreshToken.CreateRefreshToken(refreshToken, refreshTokenExpireTime);
            var newSession = new Session(
                id:id, 
                expireTime:expireTime,
                refreshToken:newToken,
                createdTime:DateTime.Now,
                updatedTime: null
                );

            newSession.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsGreaterThan(expireTime, DateTime.Now, "Session.expiredTime", "Data de expiração não posso ser menor do que a data atual"));

            newSession.AddNotifications(newToken.Notifications);

            return newSession;
        }


        public void UpdateRefreshToken(string newRefreshToken, DateTime refreshTokenExpireTime)
        {
            RefreshToken.Update(newRefreshToken, refreshTokenExpireTime);
            AddNotifications(RefreshToken.Notifications);
        }

    }
}
