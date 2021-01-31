using AutoMapper;
using Dominio.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UserCreate
    {
        public class RequestModel : IRequest<IdentityResult>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RequestModel model = null;
                RuleFor(x => x).Must(m => { model = m; return true; });

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("No se ha especificado la cuenta de correo electrónico")
                    .EmailAddress().WithMessage("Dirección de correo electrónico incorrecta")
                    .MustAsync(async (email, cancellation) => !await context.Users.AnyAsync(x => x.Email == email, cancellationToken: cancellation))
                    .WithMessage(x => $"Correo electrónico ya registrado")
                    .MustAsync(async (email, cancellation) => await context.UsuariosAsignados.AnyAsync(x => (x.Email == email) && x.Activo, cancellationToken: cancellation))
                    .WithMessage(x => $"Este correo electrónico no consta en la lista de usuarios admisibles");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Se debe registrar la contraseña")
                    .MaximumLength(30).WithMessage("El número máximo de carateres es 30")
                    .MinimumLength(6).WithMessage("La contraseña deberá contener por lo menos 6 caracteres");

                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithMessage("Se debe volver a registrar la contraseña")
                    .Must(dato => dato == model.Password).WithMessage("La contraseña y la confirmación no coinciden");
            }
        }

        public class Handler : IRequestHandler<RequestModel, IdentityResult>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ILogger<Handler> _logger;

            public Handler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<Handler> logger)
            {
                _context = context;
                _userManager = userManager;
                _logger = logger;
            }

            public async Task<IdentityResult> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                var usuarioAsignado = _context.UsuariosAsignados.First(x => x.Email == request.Email);

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    Nombre = usuarioAsignado.Nombre,
                    EmailConfirmed = true,
                    Activo = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Usuario [{request.Email}] creado");
                    return result;
                }

                throw new AppException(HttpStatusCode.NotAcceptable, result.Errors);
            }
        }

    }
}
