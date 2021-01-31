﻿using Dominio.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad.Users
{
    public class UserChangeNombre
    {
        public class RequestModel : IRequest
        {
            public string UserName { get; set; }
            public string Nombre { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(UserManager<ApplicationUser> userManager)
            {
                RuleFor(x => x.UserName)
                    .Empty().WithMessage("No se ha especificado el UserName")
                    .MustAsync(async (userName, cancellation) => (await userManager.FindByNameAsync(userName)) != null)
                    .WithMessage(x => $"Usuario no encontrado");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre no puede estar vacío")
                    .MaximumLength(100).WithMessage("El nombre debe tener más de 100 caracteres");
            }
        }

        public class Handler : IRequestHandler<RequestModel>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ILogger<Handler> _logger;

            public Handler(UserManager<ApplicationUser> userManager, ILogger<Handler> logger)
            {
                _userManager = userManager;
                _logger = logger;
            }

            public async Task<Unit> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.UserName);
                user.Nombre = request.Nombre;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Usuario [{request.UserName}] Updated");
                    return Unit.Value;
                }

                throw new AppException(HttpStatusCode.NotAcceptable, result.Errors);
            }
        }

    }
}
