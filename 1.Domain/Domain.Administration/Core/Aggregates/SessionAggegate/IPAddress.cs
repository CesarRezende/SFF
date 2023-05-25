using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel;

namespace SFF.Domain.Administration.Core.Aggregates.SessionAggegate
{
    public class IPAddress : ValueObject<IPAddress>
    {
        public string IP { get; private set; }

        public IPAddress(string ip)
        {
            IP = ip;
        }


        protected override bool EqualsCore(IPAddress other)
        {
            return IP == other.IP;
        }

        protected override int GetHashCodeCore()
        {
            return IP.GetHashCode();
        }


        public static IPAddress CreateIPAddress(string ip)
        {

            var newToken = new IPAddress(ip);
            newToken.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(ip, "IPAddress.IP", "IP é obrigatório")
               );

            return newToken;
        }
    }
}
