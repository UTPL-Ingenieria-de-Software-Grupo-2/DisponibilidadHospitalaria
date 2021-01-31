using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Disponibilidades;
using Aplicacion.Instituciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Digitador
{
    [Authorize(Roles = AppConstants.DigitadorRole)]
    public class CrearEditarDisponibilidadModel : PageModelBase
    {

        [BindProperty]
        public DisponibilidadCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public InstitucionDto Institucion { get; set; }

        [ViewData]
        public UnidadDto Unidad { get; set; }

        public async Task OnGetAsync(int unidadId)
        {
            Unidad = await Mediator.Send(new UnidadGet.RequestModel() { Id = unidadId });
            Institucion = await Mediator.Send(new InstitucionGet.RequestModel() { Id = Unidad.InstitucionId });

            Input = new DisponibilidadCreateUpdate.RequestModel()
            {
                Id = -1,
                UnidadId = unidadId,
                Fecha = DateTime.UtcNow
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Input.Fecha = DateTime.UtcNow;
                var unidad = await Mediator.Send(new UnidadGet.RequestModel() { Id = Input.UnidadId });
                Input.Disponibles = unidad.Capacidad - Input.Ocupadas;

                await Mediator.Send(Input);
                return Redirect("./Index");
            }
            else
            {
                await OnGetAsync(Input.UnidadId);
                return Page();
            }
        }
    }
}
