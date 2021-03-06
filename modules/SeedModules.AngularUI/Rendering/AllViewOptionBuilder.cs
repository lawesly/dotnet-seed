using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Seed.Environment.Plugins;
using Seed.Modules.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedModules.AngularUI.Rendering
{
    public class AllViewOptionBuilder : ViewOptionBuilder
    {
        readonly IPluginManager _pluginManager;
        readonly IViewOptionLoader _viewOptionLoader;
        readonly ILogger _logger;

        public AllViewOptionBuilder(
            IPluginManager pluginManager,
            IViewOptionLoader viewOptionLoader,
            IHostingEnvironment hostingEnvironment,
            ILogger<IViewOptionsBuilder> logger) : base(hostingEnvironment, logger)
        {
            _pluginManager = pluginManager;
            _viewOptionLoader = viewOptionLoader;
            _logger = logger;
        }

        protected override Task<IEnumerable<JObject>> GetViewOptionsAsync(RouteData routeData)
        {
            return _pluginManager.GetPlugins().InvokeAsync(descriptor => _viewOptionLoader.LoadAsync(descriptor), _logger);
        }
    }
}