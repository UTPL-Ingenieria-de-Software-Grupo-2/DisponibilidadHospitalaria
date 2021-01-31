using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Instituciones;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Digitador
{
    [Authorize(Roles = AppConstants.DigitadorRole)]
    public class IndexModel : PageModelBase
    {
        public List<UsuarioAsignadoDto> Asignaciones { get; set; }

        public List<InstitucionDto> Instituciones { get; set; }

        public UsuarioAsignadoDto Usuario => Asignaciones.First();

        public async Task OnGetAsync()
        {
            Asignaciones = await Mediator.Send(new UsuariosAsignadosList.RequestModel() { Filtro = User.Identity.Name });
            Instituciones = Asignaciones
                .Select(x => x.Institucion)
                .ToList();
        }
    }
}
