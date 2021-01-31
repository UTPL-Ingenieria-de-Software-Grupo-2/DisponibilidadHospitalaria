using MediatR;
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
    public class CiudadDelete
    {
        public class RequestInput : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<RequestInput>
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(ApplicationDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(RequestInput request, CancellationToken cancellationToken)
            {
                var model = await _context.Ciudades.FindAsync(request.Id);

                if (model == null)
                    throw new AppException(System.Net.HttpStatusCode.NotFound, $"No existe el registro de ciudad con Id={request.Id}");

                _context.Ciudades.Remove(model);

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                    _logger.LogInformation($"Ciudad Deleted [{model.Nombre}]");
                    return Unit.Value;
                }

                throw new AppException(System.Net.HttpStatusCode.Conflict, "No se ha eliminado el registro");
            }
        }

    }
}
