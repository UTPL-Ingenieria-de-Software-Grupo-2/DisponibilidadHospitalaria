using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterFromAssemblies(this IServiceCollection services, IConfiguration configuration)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.DefinedTypes.Where(t => t.Name == "ServiceRegister"))
                .ToList()
                .ForEach(x => Activator.CreateInstance(x, new object[] { services, configuration }));
        }
    }
}
