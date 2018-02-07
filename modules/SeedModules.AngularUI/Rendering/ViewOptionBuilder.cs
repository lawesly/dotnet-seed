﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Seed.Data.Extensions;
using Seed.Environment.Engine.Extensions;
using Seed.Modules.Site;
using Seed.Plugins;
using SeedModules.AngularUI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedModules.AngularUI.Rendering
{
    public class ViewOptionBuilder : IViewOptionsBuilder
    {
        readonly IMemoryCache _memoryCache;
        readonly IOptions<ViewOptions> _options;
        readonly IPluginManager _pluginManager;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly ISiteService _siteService;
        readonly ILogger _logger;

        public ViewOptionBuilder(
            IMemoryCache memoryCache,
            IOptions<ViewOptions> options,
            IPluginManager pluginManager,
            IHostingEnvironment hostingEnvironment,
            ISiteService siteService,
            ILogger<AllViewOptionBuilder> logger)
        {
            _memoryCache = memoryCache;
            _options = options;
            _pluginManager = pluginManager;
            _hostingEnvironment = hostingEnvironment;
            _siteService = siteService;
            _logger = logger;
        }

        public async Task<string> Build(RouteData routeData)
        {
            var cacheKey = BuildCacheKey(routeData);
            if (!_memoryCache.TryGetValue(cacheKey, out string optionString))
            {
                var referencies = await GetViewReferencesAsync(routeData);
                var defineOptions = new
                {
                    app = string.IsNullOrEmpty(_options.Value.App) ? "app.application" : _options.Value.App,
                    isDebug = _options.Value.IsDebug,
                    urlArgs = _options.Value.UrlArgs,
                    references = new Dictionary<string, object>(),
                    requires = new List<string>(),
                    patchs = new List<string>()
                };

                foreach (var refDefine in referencies)
                {
                    foreach (var refItem in refDefine.References)
                    {
                        defineOptions.references[refItem.Key] = refItem.Value;
                    }
                    defineOptions.requires.AddRange(refDefine.Requires);
                    defineOptions.patchs.AddRange(refDefine.Patchs);
                }
                optionString = JsonConvert.SerializeObject(defineOptions);
                _memoryCache.Set(cacheKey, optionString);
            }
            return await Task.FromResult(optionString);
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

        private async Task<IEnumerable<ViewReference>> GetViewReferencesAsync(RouteData routeData)
        {
            var routeReference = _siteService.GetSiteInfoAsync()
                .GetAwaiter()
                .GetResult()
                .As<IEnumerable<RouteViewReference>>("RouteReferences")
                .FirstOrDefault(e =>
                {
                    if (e.Route.Count != routeData.Values.Count) return false;
                    foreach (var key in e.Route.Keys)
                    {
                        if (!routeData.Values[key].Equals(e.Route[key]))
                            return false;
                    }
                    return true;
                });

            return await _pluginManager.GetPlugins().InvokeAsync(descriptor => GetViewReferences(descriptor, routeReference), _logger);
        }

        protected virtual Task<IEnumerable<ViewReference>> GetViewReferences(IPluginInfo pluginInfo, RouteViewReference references)
        {
            var uiReferences = new List<ViewReference>();

            if (references == null)
                return Task.FromResult<IEnumerable<ViewReference>>(uiReferences);

            var environment = _hostingEnvironment.IsDevelopment() ? "dev" : "dist";
            var uiFiles = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(pluginInfo.Path)
                .Where(x => !x.IsDirectory && x.Name.EndsWith($".ui.{environment}.json") && references.References.Contains(x.Name.Replace($".ui.{environment}.json", "")));

            if (uiFiles.Any())
            {
                uiReferences.AddRange(uiFiles.Select(uiDefineFile =>
                {
                    using (var fs = uiDefineFile.CreateReadStream())
                    {
                        using (var reader = new StreamReader(fs))
                        {
                            using (var jsonReader = new JsonTextReader(reader))
                            {
                                return new JsonSerializer().Deserialize<ViewReference>(jsonReader);
                            }
                        }
                    }
                }));
            }

            return Task.FromResult<IEnumerable<ViewReference>>(uiReferences);
        }
    }
}