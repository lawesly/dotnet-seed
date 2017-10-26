﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Seed.Plugins.Descriptors;
using Seed.Plugins.Feature;
using Seed.Plugins.Loader;

namespace Seed.Plugins.Extensions
{
    public static class PluginServiceCollectionExtensions
    {
        public static IServiceCollection AddPluginServices(this IServiceCollection services)
        {
            services.AddSingleton<IDescriptorProvider, DescriptorProvider>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<DescriptorOptions>, DescriptorOptionsSetup>());

            services.AddSingleton<IPluginProvider, PluginProvider>();
            services.AddSingleton<IPluginManager, PluginManager>();

            services.AddSingleton<ITypeFeatureProvider, TypeFeatureProvider>();
            services.AddSingleton<IFeaturesProvider, FeaturesProvider>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<PluginExpanderOptions>, PluginExpanderOptionsSetup>());

            services.AddSingleton<IPluginLoader, DefaultPluginLoader>();
            services.AddSingleton<IPluginLoader, PathPluginLoader>();

            services.AddSingleton<IPluginDependencyStrategy, PluginDependencyStrategy>();
            services.AddSingleton<IPluginPriorityStrategy, PluginPriorityStrategy>();

            return services;
        }

        public static IServiceCollection AddPluginManager(this IServiceCollection services)
        {
            services.TryAddTransient<IFeatureHash, FeatureHash>();

            return services;
        }

        public static IServiceCollection AddPluginLocation(this IServiceCollection services, string path)
        {
            return services.Configure<PluginExpanderOptions>(options =>
            {
                options.Options.Add(new PluginExpanderOption { Path = path });
            });
        }

        public static IServiceCollection AddDescriptorDefinition(this IServiceCollection services, string definitionName, string moduleType)
        {
            return services.Configure<DescriptorOptions>(options =>
            {
                var option = new DescriptorOption
                {
                    DescriptorFileName = definitionName,
                    TypeName = moduleType
                };

                options.Options.Add(option);
            });
        }
    }
}
