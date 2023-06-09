﻿using SFF.Infra.Core.Validations.Models;

namespace SFF.Infra.Core.Validations.Interface
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T instance);
    }
}
