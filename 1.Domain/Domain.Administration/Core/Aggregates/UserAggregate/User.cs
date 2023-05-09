using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel.Base;

namespace SFF.Domain.Administration.Core.Aggregates.UserAggregate
{

    public class User : AggregateRoot<Guid>
    {
        public string Name { get; private set; }

        public bool Active { get; private set; }

        public User(Guid id,
            string name,
            bool active,
            DateTimeOffset createdTime,
            DateTimeOffset updatedTime
            ) : base(id, createdTime, updatedTime)
        {
            Name = name;
            Active = active;
        }

        public static User CreateUser(
            string name
            )

        {

            var newUser = new User(Guid.NewGuid(),
                name: name,
                active: true,
                createdTime: DateTimeOffset.UtcNow,
                updatedTime: DateTimeOffset.UtcNow
                );

            newUser.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(newUser.Name, "User.Name", "Nome não deve ser nulo") );

            return newUser;
        }

    }
}
