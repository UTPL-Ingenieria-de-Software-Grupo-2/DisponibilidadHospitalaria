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
    public class CiudadGet
    {
        public class RequestModel : IRequest<CiudadDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, CiudadDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CiudadDto> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return await _context.Ciudades
                    .ProjectTo<CiudadDto>(_mapper.ConfigurationProvider)
                    .Where(x => x.Id == request.Id)
                    .FirstAsync();
            }
        }
    }
}
