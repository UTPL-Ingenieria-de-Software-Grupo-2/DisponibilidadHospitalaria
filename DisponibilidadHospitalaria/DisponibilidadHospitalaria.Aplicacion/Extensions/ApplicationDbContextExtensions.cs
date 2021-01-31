using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static async Task<Unit> InsertOrUpdate<TEntity>(this ApplicationDbContext context,
            object request,
            IMapper mapper,
            ILogger logger,
            CancellationToken cancellationToken
            ) where TEntity : class, new()
        {
            var data = mapper.Map<TEntity>(request);
            int id = (int)request.GetType().GetProperty("Id").GetValue(request);
            int newId = -1;

            if (id == -1)
            {
                var model = new TEntity();
                mapper.Map(data, model);
                typeof(TEntity).GetProperty("Id").SetValue(model, 0);
                var r = await context.AddAsync(model, cancellationToken);
                newId = (int)typeof(TEntity).GetProperty("Id").GetValue(r.Entity);
            }
            else
            {
                var model = await context.Set<TEntity>().FindAsync(id);
                mapper.Map(data, model);
                context.Set<TEntity>().Update(model);
            }

            var result = await context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                logger.LogInformation($"{typeof(TEntity).Name} {(id == -1 ? "Inserted" : "Updated")} [Id={(id == -1 ? newId : id)}]");
                return Unit.Value;
            }

            throw new AppException(System.Net.HttpStatusCode.Conflict, "No se realizaron los cambios");
        }
    }
}
