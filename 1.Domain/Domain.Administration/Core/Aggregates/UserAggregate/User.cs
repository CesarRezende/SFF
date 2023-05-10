using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel.Base;

namespace SFF.Domain.Administration.Core.Aggregates.UserAggregate
{

    public class User : AggregateRoot<long>
    {
        public string Login { get; private set; }
        public string Name { get; private set; }
        public bool Administrator { get; private set; }
        public bool Inactived { get; private set; }

        public User(long id,
            string name,
            string login,
            bool administrator,
            bool inactived,
            DateTimeOffset createdTime,
            DateTimeOffset? updatedTime
            ) : base(id, createdTime, updatedTime)
        {
            Login = login;
            Name = name;
            Administrator = administrator;
            Inactived = inactived;
        }

        public static User CreateUser(
            string name,
            string login,
            bool administrator
            )

        {

            var newUser = new User(default(long),
                login: login,
                name: name,
                administrator: administrator,
                inactived: false,
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
