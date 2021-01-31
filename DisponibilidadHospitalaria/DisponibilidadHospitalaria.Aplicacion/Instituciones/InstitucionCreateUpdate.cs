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

namespace Aplicacion.Instituciones
{
    public class InstitucionCreateUpdate
    {
        public class RequestModel : IRequest
        {
            public int Id { get; set; }
            public int TipoDeInstitucionId { get; set; }
            public int CiudadId { get; set; }
            public string Nombre { get; set; }
            public DireccionDto Direccion { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("No se ha especificado el campo Id")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return (id == -1) || (await context.Instituciones.AnyAsync(x => x.Id == id, cancellationToken: cancellation));
                    }).WithMessage(x => $"No existe una institucion con Id={x.Id}");

                RuleFor(x => x.TipoDeInstitucionId)
                    .NotEmpty().WithMessage("No de ha especificado el tipo de institución")
                    .MustAsync(async (tipoDeInstitucionId, cancellation) =>
                    {
                        return (await context.TiposDeInstitucion.AnyAsync(x => x.Id == tipoDeInstitucionId, cancellationToken: cancellation));
                    }).WithMessage(x => $"Tipo de institución no admisible");

                RuleFor(x => x.CiudadId)
                    .NotEmpty().WithMessage("No de ha especificado la ciudad")
                    .MustAsync(async (ciudadId, cancellation) =>
                    {
                        return (await context.Ciudades.AnyAsync(x => x.Id == ciudadId, cancellationToken: cancellation));
                    }).WithMessage(x => $"La ciudad no se ha definido correctamente");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre no puede estar vacío")
                    .MaximumLength(200).WithMessage("El nombre debe tener m[aximo 200 caracteres");

                RuleFor(x => x.Direccion)
                    .SetValidator(new DireccionValidator());

                RuleFor(x => x)
                    .MustAsync(async (model, CancellationToken) =>
                    {
                        return !await context.Instituciones.AnyAsync(x => (x.Id != model.Id) && (x.CiudadId == model.CiudadId) && (x.Nombre == model.Nombre), cancellationToken: CancellationToken);
                    }).WithMessage(x => $"Ya está definida la institución [{x.Nombre}] en la ciudad [{(context.Ciudades.FirstOrDefault(p => p.Id == x.CiudadId))?.Nombre}]");
            }
        }

        public class DireccionValidator : AbstractValidator<DireccionDto>
        {
            public DireccionValidator()
            {
                RuleFor(x => x.CallePrincipal)
                    .NotEmpty().WithMessage("Dato no admisible")
                    .MaximumLength(100).WithMessage("La longitud máxima es de 100 caracteres");

                RuleFor(x => x.CalleSecundaria)
                    .MaximumLength(100).WithMessage("La longitud máxima es de 100 caracteres");

                RuleFor(x => x.Numeracion)
                    .MaximumLength(20).WithMessage("La longitud máxima es de 20 caracteres");

                RuleFor(x => x.Longitud)
                    .MaximumLength(30).WithMessage("La longitud máxima es de 30 caracteres");

                RuleFor(x => x.Latitud)
                    .MaximumLength(30).WithMessage("La longitud máxima es de 30 caracteres");
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
                return _context.InsertOrUpdate<Institucion>(request, _mapper, _logger, cancellationToken);
            }
        }

    }
}
