using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration
{
    public static class HangfireConfig
    {
        public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();
        }
    }
}
