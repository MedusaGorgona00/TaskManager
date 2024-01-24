using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration
{
    public class ExecutionHistoriesConfiguration : IEntityTypeConfiguration<ExecutionHistory>
    {
        public void Configure(EntityTypeBuilder<ExecutionHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(ur => ur.Execution)
                .WithOne(r => r.ExecutionHistory)
                .HasForeignKey<ExecutionHistory>(ur => ur.ExecutionId);
        }
    }
}
