using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Infrastructure.Context;
using ChatInRealTime.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatInRealTime.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddAppDbContext(this IServiceCollection services, string connString)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }

        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        }

    }
}
