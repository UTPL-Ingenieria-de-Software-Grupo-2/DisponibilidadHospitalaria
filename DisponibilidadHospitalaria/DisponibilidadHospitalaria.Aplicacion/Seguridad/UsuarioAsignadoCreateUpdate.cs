using Aplicacion.Extensions;
using AutoMapper;
using Dominio.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioAsignadoCreateUpdate
    {
        public class RequestModel : IRequest
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public bool Activo { get; set; }
            public bool EsAdministrador { get; set; }
            public int InstitucionId { get; set; }

            // requerido en el view
            public int ProvinciaId { get; set; }
            public int CiudadId { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RequestModel model = null;
                RuleFor(x => x).Must(m => { model = m; return true; });

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("No se ha especificado el campo Id")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return id == -1 || (await context.UsuariosAsignados.AnyAsync(x => x.Id == id, cancellationToken: cancellation));
                    }).WithMessage(x => $"No existe un registro con Id={x.Id}");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("No se ha ingresado el nombre")
                    .MaximumLength(100).WithMessage("El nombre debe tener más de 100 caracteres");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("El e-mail no puede estar vacío")
                    .EmailAddress().WithMessage("E-mail incorrecto")
                    .MustAsync(async (email, cancellation) =>
                    {
                        return !(await context.UsuariosAsignados.AnyAsync(x => (x.Id != model.Id) && (x.Email == email), cancellationToken: cancellation));
                    }).WithMessage(x => $"E-mail ya registrado");

                RuleFor(x => x.InstitucionId)
                    .NotEmpty().WithMessage("No se ha especificado la institución")
                    .MustAsync(async (institucionId, cancellation) =>
                    {
                        return (await context.Instituciones.AnyAsync(x => x.Id == institucionId, cancellationToken: cancellation));
                    }).WithMessage(x => $"La institución no se ha definido correctamente");
            }
        }

        public class Handler : IRequestHandler<RequestModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(ApplicationDbContext context, IMapper mapper, ILogger<Handler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public Task<Unit> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                request.EsAdministrador = false;
                return _context.InsertOrUpdate<UsuarioAsignado>(request, _mapper, _logger, cancellationToken);
            }

        }
    }
}
