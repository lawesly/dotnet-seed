﻿using Microsoft.EntityFrameworkCore;
using Seed.Environment.Engine;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Data
{
    public class ModuleDbContext : DbContext, IDocumentDbContext
    {
        readonly IEnumerable<object> _entityConfigurations;
        readonly EngineSettings _settings;

        public DbContext Context => this;

        public DbSet<Document> Document { get; set; }

        public DbSet<MigrationRecord> Migrations { get; set; }

        public ModuleDbContext(
            DbContextOptions options,
            EngineSettings settings,
            params object[] entityConfigurations)
            : base(options)
        {
            _entityConfigurations = entityConfigurations;
            _settings = settings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MigrationTypeConfiguration());

            foreach (var configuration in _entityConfigurations)
            {
                modelBuilder.ApplyConfiguration((dynamic)configuration);
            }

            modelBuilder.Model
                .GetEntityTypes()
                .ToList()
                .ForEach(e => modelBuilder.Entity(e.Name).ToTable(string.Format("{0}_{1}", _settings.TablePrefix, e.Relational().TableName)));

            base.OnModelCreating(modelBuilder);
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return Model.FindEntityType(typeof(TEntity)) != null
                ? base.Set<TEntity>()
                : new DocumentDbSet<TEntity>(this);
        }
    }
}
