﻿using Microsoft.Extensions.DependencyInjection;
using Seed.Environment.Engine;
using Seed.Environment.Engine.Models;
using Seed.Modules;
using System;
using System.Threading.Tasks;

namespace Seed.Data.Migrations
{
    /// <summary>
    /// Tenant 事件中自动执行数据库迁移
    /// </summary>
    public class AutoDataMigration : IModuleTenantEvents
    {
        readonly EngineSettings _engineSettings;
        readonly IServiceProvider _serviceProvider;

        public AutoDataMigration(EngineSettings engineSettings, IServiceProvider serviceProvider)
        {
            _engineSettings = engineSettings;
            _serviceProvider = serviceProvider;
        }

        public Task ActivatedAsync()
        {
            if (_engineSettings.State != TenantStates.Uninitialized)
            {
                return _serviceProvider.GetService<IDataMigrationManager>().UpdateAllFeaturesAsync();
            }
            return Task.CompletedTask;
        }

        public Task ActivatingAsync()
        {
            return Task.CompletedTask;
        }

        public Task TerminatedAsync()
        {
            return Task.CompletedTask;
        }

        public Task TerminatingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
