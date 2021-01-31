using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Ciudades;
using Aplicacion.Instituciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades.Instituciones
{
    [Authorize(Roles = AppConstants.AdminsitradorRole)]
    public class EditarInstitucionModel : PageModelBase
    {
        [BindProperty]
        public InstitucionCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<TipoDeInstitucionDto> TiposDeInstitucion { get; set; }

        [ViewData]
        public CiudadDto Ciudad { get; set; }

        public async Task OnGetAsync(int id)
        {
            var dto = await Mediator.Send(new InstitucionGet.RequestModel() { Id = id });

            Ciudad = await Mediator.Send(new CiudadGet.RequestModel() { Id = dto.CiudadId });
            TiposDeInstitucion = await Mediator.Send(new TiposDeInstitucionList.RequestModel());
            Input ??= Mapper.Map<InstitucionCreateUpdate.RequestModel>(dto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                return Redirect($"./Index/?ciudadId={Input.CiudadId}");
            }
            else
            {
                await OnGetAsync(Input.Id);
                return Page();
            }
        }
    }
}
