﻿using SFF.Infra.Core.CQRS.Implementation;

namespace SFF.Domain.Administration.Application.Commands
{
    public class GeneratePassawordCommand : CommandBase
    {
        public string PlainPasword { get; set; }
    }
}