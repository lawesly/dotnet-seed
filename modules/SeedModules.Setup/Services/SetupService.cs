﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Seed.Data;
using Seed.Environment.Engine;
using Seed.Environment.Engine.Builder;
using Seed.Environment.Engine.Descriptors;
using Seed.Modules.DeferredTasks;
using Seed.Modules.Extensions;
using Seed.Modules.Setup.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SeedModules.Setup.Services
{
    public class SetupService : ISetupService
    {
        readonly EngineSettings _engineSettings;
        readonly IEngineContextFactory _engineContextFactory;
        readonly IEngineHost _engineHost;

        public SetupService(
            EngineSettings engineSettings,
            IEngineContextFactory engineContextFactory,
            IEngineHost engineHost)
        {
            _engineSettings = engineSettings;
            _engineContextFactory = engineContextFactory;
            _engineHost = engineHost;
        }

        public Task<string> SetupAsync(SetupContext context)
        {
            var initialState = _engineSettings.State;
            try
            {
                return ExecuteSetupAsync(context);
            }
            catch
            {
                _engineSettings.State = initialState;
                throw;
            }
        }

        private async Task<string> ExecuteSetupAsync(SetupContext context)
        {
            string[] defaultEnables =
            {
                "SeedModules.Common",
                "SeedModules.AngularUI"
            };

            context.EnabledFeatures = defaultEnables.Union(context.EnabledFeatures ?? Enumerable.Empty<string>()).Distinct().ToList();
            _engineSettings.State = LauncherStates.Initializing;

            var engineSettings = new EngineSettings(_engineSettings.Configuration);

            if (string.IsNullOrEmpty(engineSettings.DatabaseProvider))
            {
                engineSettings.ConnectionString = context.DatabaseConnectionString;
                engineSettings.DatabaseProvider = context.DatabaseProvider;
                engineSettings.TablePrefix = context.DatabaseTablePrefix;
            }

            var engineDescriptor = new EngineDescriptor()
            {
                Features = context.EnabledFeatures.Select(e => new EngineFeature(e)).ToList()
            };

            var executionId = string.Empty;

            using (var engineContext = await _engineContextFactory.CreateDescribedContextAsync(engineSettings, engineDescriptor))
            {
                using (var scope = engineContext.EntryServiceScope())
                {
                    // 初始化数据库
                    IStore store;
                    try
                    {
                        store = scope.ServiceProvider.GetRequiredService<IStore>();
                        await store.InitializeAsync();
                    }
                    catch (Exception e)
                    {
                        context.Errors.Add("DatabaseProvider", string.Format("初始化数据库访问时发生异常: {0}", e.Message));
                        return null;
                    }

                    // 刷新 EngineDescriptor
                    await scope.ServiceProvider
                        .GetService<IEngineDescriptorManager>()
                        .UpdateEngineDescriptorAsync(
                            0,
                            engineContext.Schema.Descriptor.Features,
                            engineContext.Schema.Descriptor.Parameters
                        );

                    // 后台延迟进程服务
                    var deferredTaskEngine = scope.ServiceProvider.GetService<IDeferredTaskEngine>();
                    if (deferredTaskEngine != null && deferredTaskEngine.HasPendingTasks)
                    {
                        await deferredTaskEngine.ExecuteTasksAsync(new DeferredTaskContext(scope.ServiceProvider));
                    }
                }

                executionId = Guid.NewGuid().ToString("n");
            }

            // 安装事件
            using (var shellContext = await _engineHost.CreateContextAsync(engineSettings))
            {
                using (var scope = shellContext.EntryServiceScope())
                {
                    var hasErrors = false;

                    await scope.ServiceProvider.GetServices<ISetupEventHandler>().InvokeAsync(x => x.Setup(
                        context.Name,
                        context.AdminUsername,
                        context.AdminEmail,
                        context.AdminPassword,
                        context.DatabaseProvider,
                        context.DatabaseConnectionString,
                        context.DatabaseTablePrefix,
                        (key, message) =>
                        {
                            hasErrors = true;
                            context.Errors[key] = message;
                        }
                    ), scope.ServiceProvider.GetRequiredService<ILogger<SetupService>>());

                    if (hasErrors)
                    {
                        return executionId;
                    }
                }
            }

            engineSettings.State = LauncherStates.Running;
            _engineHost.UpdateSettings(engineSettings);

            return executionId;
        }
    }
}