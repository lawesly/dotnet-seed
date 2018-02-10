﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Seed.Environment.Engine;
using Seed.Modules;
using SeedModules.Admin.Extensions;
using System;

namespace SeedModules.Admin
{
    public class Startup : StartupBase
    {
        readonly string _tenantName;
        readonly string _prefix;
        readonly IDataProtectionProvider _dataProtectionProvider;

        public Startup(EngineSettings shellSettings, IDataProtectionProvider dataProtectionProvider)
        {
            _tenantName = shellSettings.Name;
            _prefix = "/" + shellSettings.RequestUrlPrefix;
            _dataProtectionProvider = dataProtectionProvider.CreateProtector(_tenantName);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationServices(_dataProtectionProvider, _tenantName, _prefix);
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseAuthentication();
        }
    }
}
