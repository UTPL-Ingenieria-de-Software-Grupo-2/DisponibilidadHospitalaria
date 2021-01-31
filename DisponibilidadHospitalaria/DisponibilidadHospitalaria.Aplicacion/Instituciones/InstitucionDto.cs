using Aplicacion.Ciudades;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Instituciones
{
    public class InstitucionDto
    {
        public int Id { get; set; }
        public int TipoDeInstitucionId { get; set; }
        public int CiudadId { get; set; }
        public string Nombre { get; set; }
        public DireccionDto Direccion { get; set; }

        public string TipoDeInstitucion { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string ProvinciaCodigo { get; set; }
        public int ProvinciaId { get; set; }

        public List<UnidadDto> Unidades { get; set; }
    }
}
