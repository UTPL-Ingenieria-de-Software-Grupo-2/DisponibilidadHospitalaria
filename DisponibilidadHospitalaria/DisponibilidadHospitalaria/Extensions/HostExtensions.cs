using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria
{
    public static class HostExtensions
    {
        public static IHost DatabaseMigrate(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrate>();
                migrator.Migrate();
            }

            return webHost;
        }

        public static IHost DatabaseInitialize(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitialize>();
                initializer.Initialize();
            }

            return webHost;
        }
    }
}
