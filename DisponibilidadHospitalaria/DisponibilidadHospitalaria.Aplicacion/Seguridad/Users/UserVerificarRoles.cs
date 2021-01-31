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
    public class UserVerificarRoles
    {
        public class RequestModel : IRequest
        {
            public string UserName { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                RuleFor(x => x.UserName)
                    .Empty().WithMessage("No se ha especificado el UserName")
                    .MustAsync(async (userName, cancellation) => (await userManager.FindByNameAsync(userName)) != null)
                    .WithMessage(x => $"Usuario no encontrado");
            }
        }

        public class Handler : IRequestHandler<RequestModel>
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

            public async Task<Unit> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.UserName);

                if (user == null)
                    return Unit.Value;

                //Administrador
                if (await _context.UsuariosAsignados.AnyAsync(x => (x.Email == request.UserName) && (x.EsAdministrador) && (x.Activo)))
                {
                    if (!await _userManager.IsInRoleAsync(user, AppConstants.AdminsitradorRole))
                        await _userManager.AddToRoleAsync(user, AppConstants.AdminsitradorRole);
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, AppConstants.AdminsitradorRole))
                        await _userManager.RemoveFromRoleAsync(user, AppConstants.AdminsitradorRole);
                }

                //Digitador
                if (await _context.UsuariosAsignados.AnyAsync(x => (x.Email == request.UserName) && (!x.EsAdministrador) && (x.Activo)))
                {
                    if (!await _userManager.IsInRoleAsync(user, AppConstants.DigitadorRole))
                        await _userManager.AddToRoleAsync(user, AppConstants.DigitadorRole);
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, AppConstants.DigitadorRole))
                        await _userManager.RemoveFromRoleAsync(user, AppConstants.DigitadorRole);
                }

                return Unit.Value;
            }
        }

    }
}
