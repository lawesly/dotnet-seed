﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.Loader;

namespace Seed.Extensions.Plugin.Builder
{
    /// <summary>
    /// 解析一个 plugin 中所有包含的 IPlugin 接口实例
    /// </summary>
    public class PartsBuilder : BaseAssembleBuilder, IDescriptorBuilder
    {
        string _pluginPath;

        public PartsBuilder(IDescriptorBuilder builder, string pluginPath) : base(builder)
        {
            _pluginPath = pluginPath.Replace("plugin.json", "");
        }

        public override PluginDescriptor Build()
        {
            var descriptor = base.Build();
            if (!descriptor.IncludePaths.Contains(_pluginPath))
                descriptor.IncludePaths.Add(_pluginPath);
            if (descriptor.Installed)
            {
                var conventions = new ConventionBuilder();
                conventions.ForTypesDerivedFrom<IPlugin>()
                    .Export<IStartup>()
                    .Export<IPlugin>()
                    .Shared();
                var assemblies = Directory
                    .GetFiles(_pluginPath, "*.dll", SearchOption.AllDirectories)
                    .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                    .ToList();

                using (var container = new ContainerConfiguration().WithAssemblies(assemblies, conventions).CreateContainer())
                {
                    var context = new PluginRunningContext(
                        container.GetExports<IPlugin>().ToList(),
                        container.GetExports<IStartup>().ToList());
                    descriptor.Context = context;
                    //descriptor.Instances = container.GetExports<IPlugin>().ToList();
                }
            }
            return descriptor;
        }
    }
}
