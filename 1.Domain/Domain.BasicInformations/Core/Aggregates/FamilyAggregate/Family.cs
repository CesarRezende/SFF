﻿using Flunt.Notifications;
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
            DateTime createdTime,
            DateTime? updatedTime
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
                createdTime: DateTime.UtcNow,
                updatedTime: DateTime.UtcNow
                );

            newFamily.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(newFamily.Description, "Family.Description", "Descrição da familia não deve ser nulo")
               .IsGreaterOrEqualsThan(newFamily.Description.Length, 3, "Family.Description", "Descrição da familia não deve conter menos de 3 caractes"));

            return newFamily;
        }


        public void UpdateFamily(
            string description
            )

        {
            Description = description;

            AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(Description, "Family.Description", "Descrição da familia não deve ser nulo")
               .IsGreaterOrEqualsThan(Description.Length, 3, "Family.Description", "Descrição da familia não deve conter menos de 3 caractes"));

        }


        public void InactivateFamily()
        {
            Inactived = true;
        }

        public string Description { get; private set; }
        public bool Inactived { get; private set; }
    }
}
