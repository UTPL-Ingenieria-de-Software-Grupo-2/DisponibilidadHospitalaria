using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios.Master
{
    [Authorize(Roles = AppConstants.MasterRole)]
    public class CrearEditarUsuarioAdministradorModel : PageModelBase
    {

        [BindProperty]
        public UsuarioAdministradorCreateUpdate.RequestModel Input { get; set; }

        public async Task OnGetAsync(int id = -1)
        {
            Input ??= id == -1
                ? new UsuarioAdministradorCreateUpdate.RequestModel() { Id = -1, Activo = true }
                : Input = Mapper.Map<UsuarioAdministradorCreateUpdate.RequestModel>(await Mediator.Send(new UsuarioAdministradorGet.RequestModel { Id = id }));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                await Mediator.Send(new UserVerificarRoles.RequestModel() { UserName = Input.Email});
                return Redirect("./Index");
            }
            else
            {
                await OnGetAsync(Input.Id);
                return Page();
            }
        }
    }
}
