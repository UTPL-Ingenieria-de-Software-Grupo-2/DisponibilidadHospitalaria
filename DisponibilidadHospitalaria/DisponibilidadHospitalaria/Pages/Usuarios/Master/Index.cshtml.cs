using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios.Master
{
    [Authorize(Roles = AppConstants.MasterRole)]
    public class IndexModel : PageModelBase
    {
        public List<UsuarioAdministradorDto> Usuarios { get; set; }

        public async Task OnGetAsync()
        {
            Usuarios = await Mediator.Send(new UsuariosAdministradoresList.RequestModel() { Filtro = "" });
        }
    }
}
