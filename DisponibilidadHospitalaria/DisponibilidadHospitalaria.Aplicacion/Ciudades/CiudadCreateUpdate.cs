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

namespace Aplicacion.Ciudades
{
    public class CiudadCreateUpdate
    {
        public class RequestModel : IRequest
        {
            public int Id { get; set; }
            public int ProvinciaId { get; set; }
            public string Nombre { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("No se ha especificado el campo Id")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return (id == -1) || (await context.Ciudades.AnyAsync(x => x.Id == id, cancellationToken: cancellation));
                    }).WithMessage(x => $"No existe una ciudad con Id={x.Id}");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre no puede estar vacío")
                    .MaximumLength(100).WithMessage("El nombre debe tener más de 100 caracteres");

                RuleFor(x => x.ProvinciaId)
                    .NotEmpty().WithMessage("No se ha especificado la provincia")
                    .MustAsync(async (provinciaId, cancellation) =>
                    {
                        return (await context.Provincias.AnyAsync(x => x.Id == provinciaId, cancellationToken: cancellation));
                    }).WithMessage(x => $"Provincia no admisible");

                RuleFor(x => x)
                    .MustAsync(async (model, CancellationToken) =>
                    {
                        return !await context.Ciudades.AnyAsync(x => (x.Id != model.Id) && (x.ProvinciaId == model.ProvinciaId) && (x.Nombre == model.Nombre), cancellationToken: CancellationToken);
                    }).WithMessage(x => $"Ya existe la ciudad [{x.Nombre}] en la provincia [{(context.Provincias.FirstOrDefault(p => p.Id == x.ProvinciaId))?.Nombre}]");
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
                return _context.InsertOrUpdate<Ciudad>(request, _mapper, _logger, cancellationToken);
            }
        }

    }
}
