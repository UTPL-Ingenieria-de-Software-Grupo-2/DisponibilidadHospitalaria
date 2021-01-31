using Aplicacion.Ciudades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria.ViewModels
{
    public class ElgirInstitucionVM
    {
        private readonly List<CiudadDto> _ciudades;

        public ElgirInstitucionVM(List<CiudadDto> ciudades)
        {
            _ciudades = ciudades;

            ProvinciaId = Provincias.OrderBy(x => x.Codigo).FirstOrDefault()?.Id ?? -1;
            CiudadId = Ciudades.OrderBy(x => x.Nombre).FirstOrDefault()?.Id ?? -1;
            InstitucionId = Instituciones.OrderBy(x => x.Nombre).FirstOrDefault()?.Id ?? -1;
        }

        public int ProvinciaId { get; set; }

        public int CiudadId { get; set; }

        public int InstitucionId { get; set; }

        public void SetInstitucionId(int id)
        {
            var institucion = _ciudades.SelectMany(x => x.Instituciones).FirstOrDefault(x => x.Id == id);

            InstitucionId = institucion.Id;
            CiudadId = institucion.CiudadId;
            ProvinciaId = institucion.ProvinciaId;
        }

        public void SetCiudadId(int id)
        {
            var ciudad = _ciudades.FirstOrDefault(x => x.Id == id);
            CiudadId = ciudad.Id;
            ProvinciaId = ciudad.ProvinciaId;
        }

        public void SetProvinciaCiudad(int provinciaId, int ciudadId)
        {
            ProvinciaId = provinciaId;
            CiudadId = Ciudades.Any(x => x.Id == ciudadId) ? ciudadId : Ciudades.OrderBy(x => x.Nombre).First().Id;
        }

        public List<Provincia> Provincias
        {
            get
            {
                var provincias = new List<Provincia>();
                _ciudades
                    .SelectMany(x => x.Instituciones)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!provincias.Any(p => p.Id == x.ProvinciaId))
                            provincias.Add(new Provincia() { Id = x.ProvinciaId, Codigo = x.ProvinciaCodigo, Nombre = x.Provincia });
                    });

                return provincias;
            }
        }

        public List<Ciudad> Ciudades => _ciudades
            .Where(x => x.ProvinciaId == ProvinciaId)
            .Where(x => x.Instituciones.Count > 0)
            .Select(x => new Ciudad() { Id = x.Id, Nombre = x.Nombre })
            .ToList();

        public List<Institucion> Instituciones => _ciudades
            .First(x => x.Id == CiudadId).Instituciones
            .Select(x => new Institucion() { Id = x.Id, Nombre = x.Nombre })
            .ToList();

        public CiudadDto CiudadElegida => _ciudades.FirstOrDefault(x => x.Id == CiudadId);

        public class Provincia
        {
            public int Id { get; set; }
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string NombreLista => $"{Codigo} {Nombre}";
        }

        public class Ciudad
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Institucion
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }


    }
}
