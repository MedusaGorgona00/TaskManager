using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Configuration
{
    public class ExecutionsConfiguration : IEntityTypeConfiguration<Execution>
    {
        public void Configure(EntityTypeBuilder<Execution> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(ur => ur.Job)
                .WithMany(r => r.Executions)
                .HasForeignKey(ur => ur.JobId);
        }
    }
}
