using Aplicacion.Ciudades;
using DisponibilidadHospitalaria.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria.Pages
{
    public class IndexModel : PageModelBase
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int ProvinciaId { get; set; }
            public int CiudadId { get; set; }
        }

        [ViewData]
        public List<ElgirInstitucionVM.Provincia> Provincias => ViewModel.Provincias;

        [ViewData]
        public List<ElgirInstitucionVM.Ciudad> Ciudades => ViewModel.Ciudades;

        private ElgirInstitucionVM ViewModel { get; set; } = null;

        public CiudadDto Ciudad => ViewModel.CiudadElegida;

        private int CookieCiudadId
        {
            get => int.Parse(Request.Cookies["CiudadId"] ?? "-1");
            set
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(180));
                Response.Cookies.Append("CiudadId", value.ToString(), cookieOptions);
            }
        }

        public async Task OnGetAsync()
        {
            ViewModel = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel()));
            if (CookieCiudadId != -1)
                ViewModel.SetCiudadId(CookieCiudadId);

            Input = new InputModel() { ProvinciaId = ViewModel.ProvinciaId, CiudadId = ViewModel.CiudadId };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewModel = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel()));
            ViewModel.SetProvinciaCiudad(Input.ProvinciaId, Input.CiudadId);
            CookieCiudadId = Input.CiudadId;
            return Page();
        }
    }
}
