using AutoMapper;
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
    public class ProvinciasList
    {
        public class RequestModel : IRequest<List<ProvinciaDto>>
        {
        }

        public class Handler : IRequestHandler<RequestModel, List<ProvinciaDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ProvinciaDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return _mapper.Map<List<ProvinciaDto>>(await _context.Provincias.ToListAsync(cancellationToken: cancellationToken));
            }
        }
    }
}
