using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Domain.BasicInformations.Application.Queriables;
using SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Helper;
using SFF.SharedKernel.Helpers;

namespace SFF.Domain.BasicInformations.Application.AppServices
{
    public class BasicInformationsAppService : BaseAppService,  IBasicInformationsAppService
    {

        private readonly IFamilyRepository _familyRepository;
        private readonly IConfiguration _configuration;
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
            _logger = logger != null ? logger : throw new ArgumentNullException("logger");
            _configuration = configuration != null ? configuration : throw new ArgumentNullException("configuration");
        }


        public void Dispose()
        {
            _familyRepository.Dispose();
        }


        #region BasicInformationsAppService

        #region Family
        public async Task<CommandResult> InsertFamilyAsync(
            string description
            )
        {
            try
            {

                if (await _familyRepository.ExiteFamiliaAsync(description.Trim()))
                {
                    _logger.LogError($"Family {description} already exist!");
                    return Result.Failed($"Familia {description} já existe");
                }

                var newFamily = Family.CreateFamily(description);

                _logger.LogDebug($"Family: {newFamily.ToJsonFormat()}");


                if (newFamily.IsValid)
                {

                    await _familyRepository.InsertAsync(newFamily);
                    _logger.LogInformation($"Dispatching  family {newFamily.Id} domain events");
                    await DispatchEvents(newFamily.DomainEvents);

                    _logger.LogInformation($"Family {newFamily.Id} {description} inserted successfully!");
                }
                else
                {
                    _logger.LogWarning($"Family {newFamily.Id} is invalid!");
                    _logger.LogWarning(newFamily.Notifications.CreateLogMsg());
                    return Result.Failed($"Ocorreu um erro inesperado ao tentar inserir a familia. {newFamily.Notifications.CreateLogMsg()}");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while trying to insert the family", ex);
                return Result.Failed($"Ocorreu um erro inesperado ao tentar inserir a familia {description}");
            }
        }


        public async Task<CommandResult> UpdateFamilyAsync(
            long id,
            string newDescription
            )
        {

            var familyOldDescription = string.Empty;

            try
            {
                var family = await _familyRepository.GetByIdAsync(id);
                familyOldDescription = family?.Description;

                _logger.LogDebug($"Family: {family.ToJsonFormat()}");

                if (family == null)
                {
                    _logger.LogError($"Family {id} wasa not found!");
                    return Result.Failed($"Family de Id {id} não foi encontrada");
                }

                family.UpdateFamily(newDescription);

                if (family.IsValid)
                {
                    await _familyRepository.UpdateAsync(family);
                    _logger.LogInformation($"Dispatching  family {family.Id} domain events");
                    await DispatchEvents(family.DomainEvents);

                    _logger.LogInformation($"Family {family.Id} {newDescription} inserted successfully!");
                }
                else
                {
                    _logger.LogWarning($"Family {family.Id} is invalid!");
                    _logger.LogWarning(family.Notifications.CreateLogMsg());
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while trying to insert the family", ex);
                return Result.Failed($"Ocorreu um erro inesperado ao tentar atualizar a descrição da familia {familyOldDescription} para {newDescription}");
            }
        }

        public async Task<CommandResult> InactivateFamilyAsync(
            long id
            )
        {
            var familyDescription = string.Empty;

            try
            {
                var family = await _familyRepository.GetByIdAsync(id);
                familyDescription = family?.Description;

                _logger.LogDebug($"Family: {family.ToJsonFormat()}");

                if (family == null)
                {
                    _logger.LogError($"Family {id} was not found!");
                    return Result.Failed($"Family de Id {id} não foi encontrada");
                }

                family.InactivateFamily();

                if (family.IsValid)
                {
                    await _familyRepository.UpdateAsync(family);
                    _logger.LogInformation($"Dispatching  family {family.Id} domain events");
                    await DispatchEvents(family.DomainEvents);

                    _logger.LogInformation($"Family {family.Id} {family.Id} inactivated successfully!");
                }
                else
                {
                    _logger.LogWarning($"Family {family.Id} is invalid!");
                    _logger.LogWarning(family.Notifications.CreateLogMsg());
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while trying to inactivate the family Id {id}", ex);
                return Result.Failed($"Ocorreu um erro inesperado ao tentar excluir a familia Id {id} {familyDescription}");
            }
        }

        #endregion Family


        #endregion

    }
}
