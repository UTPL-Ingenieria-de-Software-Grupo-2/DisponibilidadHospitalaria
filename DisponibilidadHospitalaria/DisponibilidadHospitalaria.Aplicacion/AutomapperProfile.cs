using Aplicacion.Seguridad;
using AutoMapper;
using Dominio.Entities;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UsuarioAsignado, UsuarioAdministradorDto>();
            CreateMap<UsuarioAdministradorDto, UsuarioAdministradorCreateUpdate.RequestModel>();
            CreateMap<UsuarioAdministradorCreateUpdate.RequestModel, UsuarioAsignado>().ReverseMap();
            CreateMap<UsuarioAsignado, UsuarioAsignado>();
        }
    }
}
