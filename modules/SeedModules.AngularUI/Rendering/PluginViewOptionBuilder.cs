using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Seed.Environment.Engine;
using Seed.Environment.Plugins;
using Seed.Modules.Exceptions;
using Seed.Mvc.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedModules.AngularUI.Rendering
{
    public class PluginViewOptionBuilder : ViewOptionBuilder
    {
        readonly IViewOptionLoader _viewOptionsLoader;
        readonly IMemoryCache _memoryCache;
        readonly IEngineFeaturesManager _engineFeaturesManager;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly ILogger _logger;

        public PluginViewOptionBuilder(
            ISiteService siteService,
            IViewOptionLoader viewOptionsLoader,
            IMemoryCache memoryCache,
            IEngineFeaturesManager engineFeaturesManager,
            IHostingEnvironment hostingEnvironment,
            ILogger<IViewOptionsBuilder> logger) : base(siteService, hostingEnvironment, logger)
        {
            _viewOptionsLoader = viewOptionsLoader;
            _memoryCache = memoryCache;
            _engineFeaturesManager = engineFeaturesManager;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public override async Task<string> Build(ControllerContext controllerContext, RouteData routeData)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return await base.Build(controllerContext, routeData);
            }
            else
            {
                var cacheKey = BuildCacheKey(routeData);
                if (!_memoryCache.TryGetValue(cacheKey, out string optionString))
                {
                    optionString = await base.Build(controllerContext, routeData);
                    _memoryCache.Set(cacheKey, optionString);
                }
                return optionString;
            }
        }

        protected override async Task<IEnumerable<JObject>> GetViewOptionsAsync(RouteData routeData)
        {
            return await (await GetPluginsAsync(routeData)).InvokeAsync(descriptor => _viewOptionsLoader.LoadAsync(descriptor), _logger);
        }

        private Task<IEnumerable<IPluginInfo>> GetPluginsAsync(RouteData routeData)
        {
            var plugins = new Dictionary<string, IPluginInfo>();
            _engineFeaturesManager.GetEnabledFeaturesAsync()
               .GetAwaiter()
               .GetResult()
               .ToList()
               .ForEach(feature =>
               {
                   if (!plugins.ContainsKey(feature.Plugin.Id))
                   {
                       plugins.Add(feature.Plugin.Id, feature.Plugin);
                   }
               });
            return Task.FromResult(plugins.Values.AsEnumerable());
        }

        private string BuildCacheKey(RouteData routeData)
        {
            var keyBuilder = new StringBuilder();
            foreach (var key in routeData.Values.Keys)
            {
                keyBuilder.AppendFormat("_{0}.{1}", key, routeData.Values[key]);
            }
            return keyBuilder.ToString();
        }
    }
}