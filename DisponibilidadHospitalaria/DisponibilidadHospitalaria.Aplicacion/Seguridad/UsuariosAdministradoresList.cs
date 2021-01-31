using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuariosAdministradoresList
    {
        public class RequestModel : IRequest<List<UsuarioAdministradorDto>>
        {
            public string Filtro { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, List<UsuarioAdministradorDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuarioAdministradorDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return _mapper.Map<List<UsuarioAdministradorDto>>(await _context.UsuariosAsignados
                    .Where(x => x.EsAdministrador)
                    .Where(x => string.IsNullOrWhiteSpace(request.Filtro)
                        || x.Nombre.Contains(request.Filtro, StringComparison.InvariantCultureIgnoreCase)
                        || x.Email.Contains(request.Filtro, StringComparison.InvariantCultureIgnoreCase))
                    .ToListAsync(cancellationToken: cancellationToken));
            }
        }

    }
}
