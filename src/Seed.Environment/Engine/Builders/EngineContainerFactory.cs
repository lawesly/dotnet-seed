using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Seed.Environment.Engine.Builders.Models;
using Seed.Environment.Plugins;
using Seed.Environment.Plugins.Features;
using System;
using System.Linq;

namespace Seed.Environment.Engine.Builders
{
    public class EngineContainerFactory : IEngineContainerFactory
    {
        private readonly IFeatureInfo _applicationFeature;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceCollection _applicationServices;

        public EngineContainerFactory(
            IHostingEnvironment hostingEnvironment,
            IPluginManager pluginManager,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            ILogger<EngineContainerFactory> logger,
            IServiceCollection applicationServices)
        {
            _applicationFeature = pluginManager.GetFeatures().FirstOrDefault(
                f => f.Id == hostingEnvironment.ApplicationName);

            _applicationServices = applicationServices;
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
            _logger = logger;
        }

        public void AddCoreServices(IServiceCollection services)
        {
            services.TryAddScoped<IEngineStateUpdater, EngineStateUpdater>();
            services.TryAddScoped<IEngineStateManager, NullEngineStateManager>();
            services.AddScoped<IEngineDescriptorManagerEventHandler, EngineStateCoordinator>();
        }

        public IServiceProvider CreateContainer(EngineSettings settings, EngineSchema schema)
        {
            var tenantServiceCollection = _serviceProvider.CreateChildContainer(_applicationServices);

            tenantServiceCollection.AddSingleton(settings);
            tenantServiceCollection.AddSingleton(schema.Descriptor);
            tenantServiceCollection.AddSingleton(schema);

            AddCoreServices(tenantServiceCollection);

            var moduleServiceCollection = _serviceProvider.CreateChildContainer(_applicationServices);

            foreach (var dependency in schema.Dependencies.Where(t => typeof(Modules.IStartup).IsAssignableFrom(t.Key)))
            {
                moduleServiceCollection.AddSingleton(typeof(Modules.IStartup), dependency.Key);
                tenantServiceCollection.AddSingleton(typeof(Modules.IStartup), dependency.Key);
            }

            var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
            moduleServiceCollection.TryAddSingleton(configuration);
            tenantServiceCollection.TryAddSingleton(configuration);

            moduleServiceCollection.AddSingleton(settings);

            var moduleServiceProvider = moduleServiceCollection.BuildServiceProvider(true);

            var featureAwareServiceCollection = new FeatureAwareServiceCollection(tenantServiceCollection);

            var startups = moduleServiceProvider.GetServices<Modules.IStartup>();

            startups = startups.OrderBy(s => s.Order);

            foreach (var startup in startups)
            {
                var feature = schema.Dependencies.FirstOrDefault(x => x.Key == startup.GetType()).Value?.FeatureInfo;

                featureAwareServiceCollection.SetCurrentFeature(feature ?? _applicationFeature);
                startup.ConfigureServices(featureAwareServiceCollection);
            }

            (moduleServiceProvider as IDisposable).Dispose();

            var engineServiceProvider = tenantServiceCollection.BuildServiceProvider(true);

            var typeFeatureProvider = engineServiceProvider.GetRequiredService<ITypeFeatureProvider>();

            foreach (var featureServiceCollection in featureAwareServiceCollection.FeatureCollections)
            {
                foreach (var serviceDescriptor in featureServiceCollection.Value)
                {
                    if (serviceDescriptor.ImplementationType != null)
                    {
                        typeFeatureProvider.TryAdd(serviceDescriptor.ImplementationType, featureServiceCollection.Key);
                    }
                    else if (serviceDescriptor.ImplementationInstance != null)
                    {
                        typeFeatureProvider.TryAdd(serviceDescriptor.ImplementationInstance.GetType(), featureServiceCollection.Key);
                    }
                    else
                    {

                    }
                }
            }

            return engineServiceProvider;
        }
    }
}