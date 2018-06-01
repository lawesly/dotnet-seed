﻿using Microsoft.Extensions.DependencyInjection;
using Seed.Modules;
using SeedModules.AngularUI.Rendering;
using System;

namespace SeedModules.PageBuilder
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRouteReferenceProvider, RouteReferences>();
        }
    }
}
