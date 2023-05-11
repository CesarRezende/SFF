using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel.Base;

namespace SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate
{
    public class Family : AggregateRoot<long>
    {
        public Family(
            long id,
            string description,
            bool inactived,
            DateTimeOffset createdTime,
            DateTimeOffset? updatedTime
            ) : base(id, createdTime, updatedTime)
        {
            Id = id;
            Description = description;
            Inactived = inactived;
        }


        public static Family CreateFamily(
            string description
            )

        {

            var newFamily = new Family(default(long),
                description: description,
                inactived: false,
                createdTime: DateTimeOffset.UtcNow,
                updatedTime: DateTimeOffset.UtcNow
                );

            newFamily.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(newFamily.Description, "Family.Description", "Descrição da familia não deve ser nulo")
               .IsGreaterOrEqualsThan(newFamily.Description.Length, 3, "Family.Description", "Descrição da familia não deve conter menos de 3 caractes"));

            return newFamily;
        }
        public string Description { get; private set; }
        public bool Inactived { get; private set; }

        public DateTimeOffset createdTime { get; private set; }
        public DateTimeOffset? updatedTime { get; private set; }
    }
}
