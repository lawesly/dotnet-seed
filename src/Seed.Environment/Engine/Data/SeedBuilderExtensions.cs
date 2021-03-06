using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Seed.Environment.Engine.Descriptor;
using Seed.Modules.Builder;

namespace Seed.Environment.Engine.Data
{
    public static class SeedBuilderExtensions
    {
        public static SeedBuilder AddEngineStorage(this SeedBuilder builder)
        {
            builder.AddSitesFolder()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IEngineDescriptorManager, EngineDescriptorManager>();
                    services.AddScoped<IEngineStateManager, EngineStateManager>();
                    services.AddScoped<IEngineFeaturesManager, EngineFeaturesManager>();
                    services.AddScoped<IEngineDescriptorFeaturesManager, EngineDescriptorFeaturesManager>();
                });

            return builder;
        }

        public static SeedBuilder AddSitesFolder(this SeedBuilder builder)
        {
            var services = builder.ApplicationServices;

            services.AddSingleton<IEngineSettingsConfigurationProvider, EngineSettingsConfigurationProvider>();
            services.AddSingleton<IEngineSettingsManager, EngineSettingsManager>();
            services.AddTransient<IConfigureOptions<EngineOptions>, EngineOptionsSetup>();

            return builder;
        }
    }
}