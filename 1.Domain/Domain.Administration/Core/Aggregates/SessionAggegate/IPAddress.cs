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
    }
}
