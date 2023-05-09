using db = SFF.Infra.Repository.Entities.Administration;
using domain = SFF.Domain.Administration.Core.Aggregates.UserAggregate;

namespace SFF.Infra.Repository.MapExtension.Administration
{
    public static class UserMapExtensions
    {

        public static db.User ToDbEntity(this domain.User user)
        {

            if (user == null)
                throw new ArgumentException("Domain entity can not be null");


            var dbEntity = new db.User()
            {
                Id = user.Id,
                Name = user.Name,
                Active = user.Active,
                CreatedTime = user.CreatedTime,
                UpdatedTime = user.UpdatedTime,
            };

            return dbEntity;
        }

        public static domain.User? ToDomain(this db.User user)
        {
            if (user == null)
                return default;


            var domainEntity = new domain.User(
                id: user.Id,
                name: user.Name,
                active: user.Active,
                createdTime: user.CreatedTime,
                updatedTime: user.UpdatedTime
                );

            return domainEntity;

        }

    }
}
