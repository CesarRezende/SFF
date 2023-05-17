using SFF.Domain.BasicInformations.Application.Queriables;
using SFF.Domain.SharedKernel.Base;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.BasicInformations.Application
{

    public interface IBasicInformationsAppService : IAppService
    {
        /// <summary>
        /// used to return DTOS without pass through domain
        /// </summary>
        IBasicInformationsQueryable Query { get; }


        #region Family
        Task<CommandResult> InsertFamilyAsync(
            string description
            );

        Task<CommandResult> UpdateFamilyAsync(
            long id,
            string newDescription
            );

        Task<CommandResult> InactivateFamilyAsync(
            long id
            );


        #endregion Family



    }
}
