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

namespace Aplicacion.Seguridad
{
    public class UsuarioAdministradorGet
    {
        public class RequestModel : IRequest<UsuarioAdministradorDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, UsuarioAdministradorDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UsuarioAdministradorDto> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return _mapper.Map<UsuarioAdministradorDto>(await _context.UsuariosAsignados.Where(x => x.Id == request.Id).FirstOrDefaultAsync());
            }
        }
    }
}
