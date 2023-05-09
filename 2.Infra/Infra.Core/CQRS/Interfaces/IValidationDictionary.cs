using Flunt.Notifications;

namespace SFF.Infra.Core.CQRS.Interfaces
{
    public interface IValidationDictionary
    {
        void AddModelError(string errorMessage);

        void AddModelError(string key, string errorMessage);

        void AddModelError(IReadOnlyCollection<Notification> erros);

        bool IsValid { get; }

        IDictionary<string, string[]> Errors { get; }
        IEnumerable<string> GetMessages();
        string GetMessage();
    }
}
