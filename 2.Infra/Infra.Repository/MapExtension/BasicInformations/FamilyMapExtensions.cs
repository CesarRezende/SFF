using db = SFF.Infra.Repository.Entities.BasicInformations;
using domain = SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate;

namespace SFF.Infra.Repository.MapExtension.BasicInformations
{
    public static class FamilyMapExtensions
    {

        public static db.Family ToDbEntity(this domain.Family family)
        {

            if (family == null)
                throw new ArgumentException("Domain entity can not be null");


            var dbEntity = new db.Family()
            {
                id = family.Id,
                descricao = family.Description,
                desativado = family.Inactived,
                //CreatedTime = family.CreatedTime,
                //UpdatedTime = family.UpdatedTime,
            };

            return dbEntity;
        }

        public static domain.Family? ToDomain(this db.Family family)
        {
            if (family == null)
                return default;


            var domainEntity = new domain.Family(
                id: family.id,
                description: family.descricao,
                inactived: family.desativado,
                createdTime: DateTime.Now, //family.createdTime,
                updatedTime: null   //family.updatedTime
                ) ;;

            return domainEntity;

        }

    }
}
