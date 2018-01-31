﻿using Seed.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Internal;

namespace Seed.Mvc
{
    public class ModuleTenantRouteBuilder : IModuleTenantRouteBuilder
    {
        readonly IServiceProvider _serviceProvider;

        public ModuleTenantRouteBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRouteBuilder Build()
        {
            return new RouteBuilder(new ApplicationBuilder(_serviceProvider))
            {
                DefaultHandler = _serviceProvider.GetRequiredService<MvcRouteHandler>()
            };
        }

        public void Configure(IRouteBuilder builder)
        {
            builder.Routes.Add(new Route(
                builder.DefaultHandler,
                "Default", "{area:exists}/{controller}/{action}/{id?}",
                null,
                null,
                null,
                _serviceProvider.GetService<IInlineConstraintResolver>()));
            builder.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(_serviceProvider));
        }
    }
}