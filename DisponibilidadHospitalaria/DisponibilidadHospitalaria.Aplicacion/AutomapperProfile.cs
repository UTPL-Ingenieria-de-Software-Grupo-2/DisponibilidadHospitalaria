using Aplicacion.Ciudades;
using Aplicacion.Instituciones;
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

            CreateMap<UsuarioAsignado, UsuarioAsignadoDto>()
                .ForMember(x => x.Institucion_Nombre, o => o.MapFrom(s => s.Institucion.Nombre))
                .ForMember(x => x.Institucion_Ciudad, o => o.MapFrom(s => s.Institucion.Ciudad.Nombre))
                .ForMember(x => x.Institucion_Provincia, o => o.MapFrom(s => s.Institucion.Ciudad.Provincia.Nombre));

            CreateMap<UsuarioAsignadoDto, UsuarioAsignadoCreateUpdate.RequestModel>();
            CreateMap<UsuarioAsignadoCreateUpdate.RequestModel, UsuarioAsignado>().ReverseMap();

            CreateMap<Provincia, ProvinciaDto>();

            CreateMap<Ciudad, CiudadDto>()
                .ForMember(x => x.Provincia_Codigo, o => o.MapFrom(s => s.Provincia.Codigo))
                .ForMember(x => x.Provincia_Nombre, o => o.MapFrom(s => s.Provincia.Nombre));

            CreateMap<CiudadDto, CiudadCreateUpdate.RequestModel>();
            CreateMap<CiudadCreateUpdate.RequestModel, Ciudad>();
            CreateMap<Ciudad, Ciudad>();

            CreateMap<Institucion, InstitucionDto>()
                .ForMember(x => x.TipoDeInstitucion, o => o.MapFrom(s => s.TipoDeInstitucion.Denominacion))
                .ForMember(x => x.Ciudad, o => o.MapFrom(s => s.Ciudad.Nombre))
                .ForMember(x => x.Provincia, o => o.MapFrom(s => s.Ciudad.Provincia.Nombre))
                .ForMember(x => x.ProvinciaId, o => o.MapFrom(s => s.Ciudad.Provincia.Id))
                .ForMember(x => x.ProvinciaCodigo, o => o.MapFrom(s => s.Ciudad.Provincia.Codigo));

            CreateMap<InstitucionDto, InstitucionCreateUpdate.RequestModel>();
            CreateMap<InstitucionCreateUpdate.RequestModel, Institucion>();
            CreateMap<Institucion, Institucion>();

            CreateMap<Direccion, DireccionDto>().ReverseMap();
            CreateMap<TipoDeInstitucion, TipoDeInstitucionDto>().ReverseMap();
            CreateMap<TipoDeUnidad, TipoDeUnidadDto>().ReverseMap();

            CreateMap<Unidad, UnidadDto>()
                .ForMember(x => x.TipoDeUnidad, o => o.MapFrom(s => s.TipoDeUnidad.Denominacion));

            CreateMap<UnidadDto, UnidadCreateUpdate.RequestModel>();
            CreateMap<UnidadCreateUpdate.RequestModel, Unidad>().ReverseMap();
            CreateMap<Unidad, Unidad>();

        }
    }
}
