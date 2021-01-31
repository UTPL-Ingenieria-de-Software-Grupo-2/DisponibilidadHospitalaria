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

namespace Aplicacion.Seguridad.Users
{
    public class UserLogin
    {
        public class RequestModel : IRequest<SignInResult>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            {
                RequestModel model = null;
                RuleFor(x => x).Must(m => { model = m; return true; });

                RuleFor(x => x.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Ingrese la cuenta")
                    .EmailAddress().WithMessage("Cuenta incorrecta")
                    .MustAsync(async (email, cancellation) =>
                    {
                        return await userManager.FindByEmailAsync(email) != null;

                    }).WithMessage("Usuario no registrado")
                    .MustAsync(async (email, cancellation) =>
                    {
                        return (await userManager.FindByEmailAsync(email))?.LockoutEnabled ?? false;

                    }).WithMessage("Usuario bloqueado");

                RuleFor(x => x.Password)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Ingrese la contraseña")
                    .MustAsync(async (password, cancellation) =>
                    {
                        var user = await userManager.FindByEmailAsync(model.Email);
                        return (user == null) || await userManager.CheckPasswordAsync(user, password);
                    }).WithMessage("Contraseña incorrecta");

                WhenAsync(async (model, CancellationToken) =>
                    {
                        var user = await userManager.FindByEmailAsync(model.Email);
                        return (user != null) && !await userManager.IsInRoleAsync(user, AppConstants.MasterRole);
                    },
                    () =>
                    {
                        RuleFor(x => x.Email)
                        .MustAsync(async (email, cancellation) =>
                        {
                            return (await context.UsuariosAsignados.FirstOrDefaultAsync(x => x.Email == email))?.Activo ?? false;
                        }).WithMessage("Cuenta de Usuario Inactiva");
                    });

                WhenAsync(async (model, CancellationToken) => !(await context.UsuariosAsignados.FirstOrDefaultAsync(x => x.Email == model.Email, cancellationToken: CancellationToken))?.EsAdministrador ?? false,
                    () =>
                    {
                        RuleFor(x => x.Email)
                        .MustAsync(async (email, cancellation) =>
                        {
                            return await context.UsuariosAsignados.FirstOrDefaultAsync(x => x.Email == email) != null;
                        }).WithMessage("No está asignado a una institución");
                    });
            }
        }

        public class Handler : IRequestHandler<RequestModel, SignInResult>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ILogger<Handler> _logger;

            public Handler(SignInManager<ApplicationUser> signInManager, ILogger<Handler> logger)
            {
                _signInManager = signInManager;
                _logger = logger;
            }

            public async Task<SignInResult> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Sesión Iniciada. Usuario [{request.Email}]");
                    return result;
                }

                throw new AppException(HttpStatusCode.Conflict, $"No se inició la sesión usuario: {request.Email}");
            }
        }
    }
}
