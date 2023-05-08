using Flunt.Notifications;
using SFF.Domain.SharedKernel;

namespace SFF.SharedKernel.ValueObjects
{
    public class Messages : ValueObject
    {
        public IReadOnlyList<string> Msgs { get => _msgs; }
        private List<string> _msgs { get; set; }

        public Messages()
        {
            _msgs = new List<string>();
        }

        public Messages(IEnumerable<string> msgs)
        {
            _msgs = msgs.ToList();
        }

        public Messages(IEnumerable<Notification> msgs)
        {
            _msgs = msgs.Select(x => x.Message).ToList();
        }


    }
}
