
using Core.Interfaces.Repository;
using Core.Interfaces.SeedDatabase;
using Core.Interfaces.Services;
using Repository;
using Services;
using Services.Jobs;
using Services.Jobs.Interfaces;
using Services.Seed;

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


            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMerchService, MerchService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IVerificationService, VerificationService>();
        }
    }
}
