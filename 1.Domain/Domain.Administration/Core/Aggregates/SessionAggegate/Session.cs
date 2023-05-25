using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel.Base;
using SFF.Domain.SharedKernel.Entities;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Domain.Administration.Core.Aggregates.SessionAggegate
{
    public class Session : AggregateRoot<long>
    {

        public IPAddress IPAddress { get; private set; }
        public DateTime ExpireTime { get; private set; }

        public RefreshToken RefreshToken { get; private set; }
        public bool IsExpired { get { return ExpireTime < DateTime.Now; } }

        public UserBasicInfo User { get; private set; }

    public Session(
            long id,
            UserBasicInfo user,
            IPAddress ipAddress,
            DateTime expireTime,
            RefreshToken refreshToken,
            DateTime createdTime,
            DateTime? updatedTime)
            : base(id, createdTime, updatedTime)
        {
            User = user;
            IPAddress = ipAddress;
            ExpireTime = expireTime;
            RefreshToken = refreshToken;
        }


        public static Session CreateSession(
            string ip,
            long userId,
            DateTime expireTime,
            string refreshToken,
            DateTime refreshTokenExpireTime)
        {

            var newToken =  RefreshToken.CreateRefreshToken(refreshToken, refreshTokenExpireTime);
            var ipAddress =  IPAddress.CreateIPAddress(ip);
            var newSession = new Session(
                id: 0,
                user: new UserBasicInfo(userId),
                ipAddress: ipAddress,
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


        public void UpdateSession(DateTime expireTime, string newRefreshToken, DateTime refreshTokenExpireTime)
        {
            ExpireTime = expireTime;
            RefreshToken.Update(newRefreshToken, refreshTokenExpireTime);
            AddNotifications(RefreshToken.Notifications);
        }

        public ValidationResult ValidateSession() {
            var validateResult = new ValidationResult();

            if (IsExpired || RefreshToken.IsExpired)
                validateResult.AddNotification("Session.Expired", $"Refresh token invalido ou expirado!");

            return validateResult;
        }

    }
}
