using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Seed.Modules.Manifest;
using Seed.Environment.Plugins;

namespace Seed.Modules
{
    public class AssemblyAttributeModuleNamesProvider : IModuleNamesProvider
    {
        private readonly List<string> _moduleNames;

        public AssemblyAttributeModuleNamesProvider(IHostingEnvironment hostingEnvironment)
        {
            var assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
            _moduleNames = assembly.GetCustomAttributes<ModuleNameAttribute>().Select(m => m.Name).ToList();
        }

        public IEnumerable<string> GetModuleNames()
        {
            return _moduleNames;
        }
    }
}