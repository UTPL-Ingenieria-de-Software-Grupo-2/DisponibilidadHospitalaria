using Aplicacion.Disponibilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Instituciones
{
    public class UnidadDto
    {
        public int Id { get; set; }
        public int InstitucionId { get; set; }
        public int TipoDeUnidadId { get; set; }
        public string Denominacion { get; set; }
        public int Capacidad { get; set; }
        public string TipoDeUnidad { get; set; }

        public List<DisponibilidadDto> UltimasDisponibilidades { get; set; }
        public DisponibilidadDto UltimaDisponibilidad => UltimasDisponibilidades.FirstOrDefault() ?? new DisponibilidadDto();
    }
}
