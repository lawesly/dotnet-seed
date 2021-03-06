using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seed.Data
{
    public class MigrationTypeConfiguration : IEntityTypeConfiguration<MigrationRecord>
    {
        public void Configure(EntityTypeBuilder<MigrationRecord> builder)
        {
            builder.ToTable("_MigrationRecord")
                .HasKey(e => e.Id);
        }
    }
}