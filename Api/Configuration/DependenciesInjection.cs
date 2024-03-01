
using Core.Interfaces.Repository;
using Core.Interfaces.SeedDatabase;
using Core.Interfaces.Services;
using MediatR;
using Repository;
using Services;
using Services.Seed;
using System;

namespace Api.Configuration
{
    public static class DependenciesInjection
    {
        public static void AddDependenciesInjection(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPlaneService, PlaneService>();
            services.AddScoped<IReservationService, ReservationService>();

            services.AddScoped<IInitSeedService, InitSeedService>();
            services.AddMediatR(Cfg => Cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }
    }
}
