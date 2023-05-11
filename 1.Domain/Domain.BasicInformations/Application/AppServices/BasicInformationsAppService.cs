using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Domain.BasicInformations.Application.Queriables;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.BasicInformations.Application.AppServices
{
    public class BasicInformationsAppService : IBasicInformationsAppService
    {

        private readonly IEventDispatcher _dispatcher;
        private readonly IFamilyRepository _familyRepository;
        private IConfiguration _configuration;
        private readonly ILogger<BasicInformationsAppService> _logger;

        public IBasicInformationsQueryable Query { get; }

        public BasicInformationsAppService(
            IBasicInformationsQueryable queryable,
            IFamilyRepository familyRepository,
            IEventDispatcher dispatcher,
            ILogger<BasicInformationsAppService> logger,
            IConfiguration configuration)
        {

            Query = queryable != null ? queryable : throw new ArgumentNullException("queryable");
            _familyRepository = familyRepository != null ? familyRepository : throw new ArgumentNullException("familyRepository");
            _dispatcher = dispatcher != null ? dispatcher : throw new ArgumentNullException("dispatcher");
            _logger = logger != null ? logger : throw new ArgumentNullException("logger");
            _configuration = configuration != null ? configuration : throw new ArgumentNullException("configuration");
        }


        public void Dispose()
        {
            _familyRepository.Dispose();
        }


        #region BasicInformationsAppService

        #region Family
        //public async Task<Result> InsertFamilyAsync(
        //    string fullName,
        //    string phoneNumber,
        //    int retailerId,
        //    int routingNumber,
        //    long bankAccountNumber,
        //    string code,
        //    string encryptedCode

        //    )
        //{
        //    try
        //    {
        //        var result = new Result();

        //        var isValidationCodeValidOp = await ValidateValidationCodeAsync(code, encryptedCode);

        //        if (!isValidationCodeValidOp.IsValid)
        //            return new Result().AddErrors(isValidationCodeValidOp.Notifications.ToList());

        //        var isvalidationCodeValid = isValidationCodeValidOp.Data;
        //        if (isvalidationCodeValid)
        //        {

        //            //Validar se retailer id existe no sistema da KornerStone
        //            var retailer = await _kornerstoneRetailerApi.GetRetailerInformation(retailerId);

        //            if (retailer.IsValid)
        //            {
        //                var retailerGroup = await _kornerstoneRetailerApi.GetRetailerGroupInformation(retailerId);

        //                if (retailerGroup.IsValid || retailerGroup.IsNotFound)
        //                {
        //                    var retailersIds = retailerGroup.Data.Select(x => x.Id);

        //                    var newFamily = Family.CreateFamily(
        //                        fullName: fullName,
        //                        phoneNumber: phoneNumber,
        //                        defaultRetailerId: retailerId,
        //                        retailersIds: retailersIds,
        //                        routingNumber: routingNumber,
        //                        bankAccountNumber: bankAccountNumber
        //                        );

        //                    _logger.LogDebug($"Family: {newFamily.ToJsonFormat()}");


        //                    if (newFamily.IsValid)
        //                    {

        //                        if (await _familyRepository.Exists(newFamily.PhoneNumber.ToInternationalFormat()))
        //                        {
        //                            var errorMsg = $"Already existe a family associate to the phone number {phoneNumber}";
        //                            _logger.LogWarning(errorMsg);

        //                            return result.AddError(MessagesHelper.GetMessage("PhoneAlreadyAssociateToFamilyErrorMsg", phoneNumber));
        //                        }

        //                        await _familyRepository.InsertAsync(newFamily);
        //                        _logger.LogInformation($"Dispatching  family {newFamily.Id} domain events");
        //                        await _dispatcher.DispatchAll(newFamily.DomainEvents);

        //                        _logger.LogInformation($"Family {newFamily.Id} inserted successfully!");
        //                    }
        //                    else
        //                    {
        //                        _logger.LogWarning($"Family {newFamily.Id} is invalid!");
        //                        _logger.LogWarning(newFamily.Notifications.CreateLogMsg());
        //                    }

        //                    return new Result(newFamily.Notifications);

        //                }
        //                else
        //                {
        //                    _logger.LogWarning($"An unexpected error occurred while trying to insert the family");
        //                    return new Result().AddErrors(retailerGroup.Notifications.ToList());
        //                }
        //            }
        //            else if (retailer.IsNotFound)
        //            {
        //                _logger.LogWarning($"Could not find retailer {retailerId}");
        //                return new Result().SetAsNotFound().AddErrors(retailer.Notifications.ToList());
        //            }
        //            else
        //            {
        //                _logger.LogWarning($"An unexpected error occurred while trying to insert the family");
        //                return new Result().AddErrors(retailer.Notifications.ToList());
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Validation code is invalid or expired");
        //            return new Result().AddError(MessagesHelper.GetMessage("CodeInvalidOrExpiredErrorMsg"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred while trying to insert the family");
        //        _logger.LogError(ex.ToString());

        //        return new Result().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorInsertFamilyErrorMsg"));
        //    }
        //}


        //public async Task<Result<FamilyAuthInformation>> GetFamilyInformationForAuth(string phoneNumber)
        //{
        //    var familyAuthInformationDTO = new Result<FamilyAuthInformation>();

        //    familyAuthInformationDTO.Data = await Query.GetFamilyAuthInformation(phoneNumber);

        //    if (familyAuthInformationDTO.Data is null)
        //    {
        //        _logger.LogWarning($"Family not found");
        //        return new Result<FamilyAuthInformation>().AddError(MessagesHelper.GetMessage("FamilyNotFoundMSg")).SetAsNotFound();
        //    }

        //    var familyLanguage = await Query.GetFamilyLanguage(familyAuthInformationDTO.Data.Id);
        //    familyAuthInformationDTO.Data.Language = familyLanguage == null ? "en-US" : familyLanguage;

        //    var retailerInfo = await _kornerstoneRetailerApi.GetRetailerInformation(familyAuthInformationDTO.Data.DefaultRetailer.Id);

        //    if (retailerInfo.IsValid && !retailerInfo.IsNotFound)
        //    {
        //        familyAuthInformationDTO.Data.DefaultRetailer.Name = retailerInfo?.Data?.Name;
        //        familyAuthInformationDTO.Data.DefaultRetailer.StateId = retailerInfo.Data?.Address?.StateId;

        //        var retailerGroup = await _kornerstoneRetailerApi.GetRetailerGroupInformation(familyAuthInformationDTO.Data.DefaultRetailer.Id);

        //        if (retailerGroup.Succeded && retailerGroup.Data.Count() != 0)
        //            foreach (var retailer in familyAuthInformationDTO.Data.Retailers)
        //            {
        //                retailer.Name = retailerGroup.Data.FirstOrDefault(x => x.Id == retailer.Id)?.Name;
        //            }


        //        familyAuthInformationDTO.Data.RaffleConfig = await Query.GetRaffleConfigInfo(retailerInfo.Data.Address.StateId);

        //        return familyAuthInformationDTO;
        //    }
        //    else
        //    {
        //        _logger.LogWarning($"An unexpected error occurred while trying to get family information for auth");
        //        return new Result<FamilyAuthInformation>().SetAsInternalServerError().AddErrors(retailerInfo.Notifications.ToList());
        //    }
        //}

        #endregion Family

        #region TokenAuth


        //public async Task<Result<Auth>> Authenticate(string phoneNumber, string code, string encryptedCode, string expoToken)
        //{
        //    try
        //    {
        //        var isValidationCodeValidOP = await ValidateValidationCodeAsync(code, encryptedCode);

        //        if (!isValidationCodeValidOP.IsValid)
        //            return new Result<Auth>().AddErrors(isValidationCodeValidOP.Notifications.ToList());

        //        var isValidationCodeValid = isValidationCodeValidOP.Data;
        //        if (isValidationCodeValid)
        //        {

        //            var phoneSuccssedparsed = phoneNumber.TryParseToPhoneNumber(out var phone);

        //            if (!phoneSuccssedparsed)
        //            {
        //                _logger.LogWarning($"PhoneNumber is invalid");
        //                return new Result<Auth>().AddError(MessagesHelper.GetMessage("PhoneNumberNotValidErrorMsg"));
        //            }

        //            var familyAuthInformation = await GetFamilyInformationForAuth(phone.ToInternationalFormat());

        //            if (!familyAuthInformation.Succeded)
        //            {
        //                if (familyAuthInformation.IsNotFound)
        //                {
        //                    return new Result<Auth>().SetAsNotFound().AddError(MessagesHelper.GetMessage("NotFoundFamilyInfoByPhoneErrorMsg", phoneNumber));
        //                }
        //                else
        //                {
        //                    return new Result<Auth>().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorLoginErrorMsg"));
        //                }
        //            }

        //            var refreshToken = _tokenService.GenerateRefreshToken();
        //            var token = _tokenService.GenerateToken(refreshToken, language: familyAuthInformation.Data.Language, family: familyAuthInformation.Data);
        //            var retailerID = familyAuthInformation.Data.DefaultRetailer.Id.ToString();
        //            var fullName = familyAuthInformation.Data.FullName;

        //            var authData = new Auth(
        //                token: token,
        //                refreshToken: refreshToken,
        //                retailerID: retailerID,
        //                fullName: fullName
        //                );

        //            var saveToken = await SaveAuthTokenAsync(authData.RefreshToken, familyAuthInformation.Data.Id, MessagesHelper.GetAppLanguage(familyAuthInformation.Data.Language), expoToken);

        //            if (!saveToken.IsValid)
        //            {
        //                return new Result<Auth>().AddErrors(saveToken.Notifications.Select(x => x.Message));
        //            }

        //            return new Result<Auth>(authData);
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Validation code is invalid or is expired");
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("CodeInvalidOrExpiredErrorMsg"));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred during the login");
        //        return new Result<Auth>().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorLoginErrorMsg"));
        //    }
        //}

        //public async Task<Result<Auth>> RefreshToken(string token, string refreshToken, string expoToken)
        //{
        //    try
        //    {
        //        var principal = _tokenService.GetPrincipalFromExpiredToken(token);
        //        Guid familyId = Guid.Parse(principal.FindFirst(ClaimTypes.PrimarySid).Value);

        //        var getSavedRefreshToken = await this.Query.GetFamilyToken(familyId);

        //        if (getSavedRefreshToken.SecurityStamp != refreshToken)
        //        {
        //            _logger.LogError("An error ocurred in Refresh Endpoint: Invalid refresh token.");
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("InvalidRefreshTokenErrorMsg"));
        //        }

        //        var newRefreshToken = _tokenService.GenerateRefreshToken();
        //        var newJwtToken = _tokenService.GenerateToken(newRefreshToken, getSavedRefreshToken.Language, claims: principal.Claims.ToList());

        //        var saveToken = await this.SaveAuthTokenAsync(newRefreshToken, familyId, MessagesHelper.GetAppLanguage(getSavedRefreshToken.Language), expoToken);

        //        if (!saveToken.IsValid)
        //        {
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorMsg"));
        //        }

        //        var auth = new Auth(newJwtToken, newRefreshToken);

        //        return new Result<Auth>(auth);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred during the renew of refresh token");
        //        return new Result<Auth>().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorMsg"));
        //    }
        //}

        #endregion

        #endregion

    }
}
