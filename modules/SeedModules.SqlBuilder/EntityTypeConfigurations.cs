using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seed.Data;
using SeedModules.SqlBuilder.Domain;

namespace SeedModules.SqlBuilder
{
    public class EntityTypeConfigurations : IEntityTypeConfigurationProvider
    {
        public async Task<IEnumerable<object>> GetEntityTypeConfigurationsAsync()
        {
            return await Task.FromResult(new object[]
            {
                new SqlBuilderPathTypeConfiguration(),
                new SqlDefineTypeConfiguration()
            });
        }
    }
}