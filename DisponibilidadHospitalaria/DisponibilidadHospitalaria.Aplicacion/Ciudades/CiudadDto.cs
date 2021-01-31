using Aplicacion.Instituciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Ciudades
{
    public class CiudadDto
    {
        public int Id { get; set; }
        public int ProvinciaId { get; set; }
        public string Nombre { get; set; }
        public string Provincia_Codigo { get; set; }
        public string Provincia_Nombre { get; set; }
        public List<InstitucionDto> Instituciones { get; set; }
    }
}
