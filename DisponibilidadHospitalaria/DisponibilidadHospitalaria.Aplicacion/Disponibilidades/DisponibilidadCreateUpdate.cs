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

namespace Aplicacion.Disponibilidades
{
    public class DisponibilidadCreateUpdate
    {
        public class RequestModel : IRequest
        {
            public int Id { get; set; }
            public int UnidadId { get; set; }
            public DateTime Fecha { get; set; }
            public int Ocupadas { get; set; }
            public int Disponibles { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RuleFor(x => x.Id)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("No se ha especificado el campo Id")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return (id == -1) || (await context.Disponibilidades.AnyAsync(x => x.Id == id, cancellationToken: cancellation));
                    }).WithMessage(x => $"No existe un regisro con Id={x.Id}");

                RuleFor(x => x.UnidadId)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("No se ha especificado la unidad")
                    .MustAsync(async (unidadId, cancellation) =>
                    {
                        return (await context.Unidades.AnyAsync(x => x.Id == unidadId, cancellationToken: cancellation));
                    }).WithMessage(x => $"Unidad no admisible");

                RuleFor(x => x.Fecha)
                    .NotEmpty().WithMessage("No se ha registrado la fecha");

                /*
                RuleFor(x => x)
                    .Must(model => (model.Ocupadas + model.Disponibles) > 0).WithMessage("La suma de ocupadas más disponibles no puede ser cero")
                    .MustAsync(async (model, cancellation) =>
                    {
                        var unidad = await context.Unidades.FindAsync(model.UnidadId);
                        return model.Ocupadas + model.Disponibles == unidad.Capacidad;
                    }).WithMessage(x => $"La suma de ocupadas más disponibles es diferente a la capacidad de la unidad");
                */
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
                    return _context.InsertOrUpdate<Disponibilidad>(request, _mapper, _logger, cancellationToken);
                }
            }
        }
    }
}
