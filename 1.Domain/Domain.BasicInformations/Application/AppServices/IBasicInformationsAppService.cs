using SFF.Domain.BasicInformations.Application.Queriables;
using SFF.Domain.SharedKernel.Base;

namespace SFF.Domain.BasicInformations.Application
{

    public interface IBasicInformationsAppService : IAppService
    {
        /// <summary>
        /// used to return DTOS without pass through domain
        /// </summary>
        IBasicInformationsQueryable Query { get; }


        #region Family
        //Task<Result> InsertFamilyAsync(
        //    string fullName,
        //    string phoneNumber,
        //    int retailerId,
        //    int routingNumber,
        //    long bankAccountNumber,
        //    string code,
        //    string encryptedCode
        //    );

        //Task<Result> UpdateFamilyPhoneNumberAsync(
        //    Guid userId,
        //    string newPhoneNumber,
        //    string code,
        //    string encryptedCode
        //    );

        //Task<Result<Family>> GetFamilyByIdAsync(Guid id);



        #endregion Family



    }
}
