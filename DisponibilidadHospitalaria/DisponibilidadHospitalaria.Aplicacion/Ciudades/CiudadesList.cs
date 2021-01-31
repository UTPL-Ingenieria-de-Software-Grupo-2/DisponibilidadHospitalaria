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

namespace Aplicacion.Ciudades
{
    public class CiudadesList
    {
        public class RequestModel : IRequest<List<CiudadDto>>
        {
            public string Filtro { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, List<CiudadDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CiudadDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return await _context.Ciudades
                    .ProjectTo<CiudadDto>(_mapper.ConfigurationProvider)
                    //.Where(x => string.IsNullOrWhiteSpace(request.Filtro) || x.Nombre.Contains(request.Filtro, StringComparison.InvariantCultureIgnoreCase))
                    .ToListAsync();
            }
        }
    }
}
