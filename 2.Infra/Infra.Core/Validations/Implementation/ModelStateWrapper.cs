using SFF.Infra.Core.CQRS.Interfaces;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFF.Infra.Core.Validations.Interface;

namespace SFF.Infra.Core.Validations.Implementation
{
    public class ModelStateWrapper : IValidationDictionary
    {
        private readonly ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        #region IValidationDictionary Members

        public void AddModelError(string errorMessage)
        {
            _modelState.AddModelError("", errorMessage);
        }

        public void AddModelError(IReadOnlyCollection<Notification> validationResults)
        {
            if (validationResults != null && validationResults.Any())
                foreach (var error in validationResults)
                    AddModelError(error.Key, error.Message);
        }

        public void AddModelError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        public IDictionary<string, string[]> Errors
        {
            get
            {
                var errorList = _modelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray()
                );

                return errorList;
            }
        }

        public IEnumerable<string> GetMessages()
        {
            return _modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
        }
        public string GetMessage()
        {
            return GetMessages().FirstOrDefault();
        }

        #endregion
    }
}
