﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Seed.Environment.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seed.Modules
{
    public class ModuleTenantRouterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dictionary<string, RequestDelegate> _pipelines = new Dictionary<string, RequestDelegate>();

        public ModuleTenantRouterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var engineSettings = httpContext.Features.Get<EngineSettings>();

            if (!string.IsNullOrEmpty(engineSettings.RequestUrlPrefix))
            {
                httpContext.Request.PathBase += ("/" + engineSettings.RequestUrlPrefix);
                httpContext.Request.Path = httpContext.Request.Path.ToString().Substring(httpContext.Request.PathBase.Value.Length);
            }

            var rebuildPipeline = httpContext.Items["BuildPipeline"] != null;
            if (rebuildPipeline && _pipelines.ContainsKey(engineSettings.Name))
            {
                _pipelines.Remove(engineSettings.Name);
            }

            if (!_pipelines.TryGetValue(engineSettings.Name, out RequestDelegate pipeline))
            {
                lock (_pipelines)
                {
                    if (!_pipelines.TryGetValue(engineSettings.Name, out pipeline))
                    {
                        pipeline = BuildTenantPipeline(engineSettings, httpContext.RequestServices);

                        if (engineSettings.State == TenantStates.Running)
                        {
                            _pipelines.Add(engineSettings.Name, pipeline);
                        }
                    }
                }
            }

            await pipeline.Invoke(httpContext);
        }

        public RequestDelegate BuildTenantPipeline(EngineSettings settings, IServiceProvider serviceProvider)
        {
            var startups = serviceProvider.GetServices<IStartup>();

            startups = startups.OrderBy(s => s.Order);

            var tenantRouteBuilder = serviceProvider.GetService<IModuleTenantRouteBuilder>();

            var appBuilder = new ApplicationBuilder(serviceProvider);
            var routeBuilder = tenantRouteBuilder.Build();

            foreach (var startup in startups)
            {
                startup.Configure(appBuilder, routeBuilder, serviceProvider);
            }

            tenantRouteBuilder.Configure(routeBuilder);

            var router = routeBuilder.Build();

            appBuilder.UseRouter(router);

            var pipeline = appBuilder.Build();

            return pipeline;
        }
    }
}