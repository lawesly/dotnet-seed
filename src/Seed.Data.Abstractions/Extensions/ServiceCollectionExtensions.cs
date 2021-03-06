﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddDataProvider(this IServiceCollection services, string name, string provider)
        {
            for (var i = services.Count - 1; i >= 0; i--)
            {
                var entry = services[i];
                if (entry.ImplementationInstance != null)
                {
                    if (entry.ImplementationInstance is DatabaseProvider databaseProvider
                        && String.Equals(databaseProvider.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        services.RemoveAt(i);
                    }
                }
            }

            services.AddSingleton(new DatabaseProvider { Name = name, Provider = provider });

            return services;
        }
    }
}
