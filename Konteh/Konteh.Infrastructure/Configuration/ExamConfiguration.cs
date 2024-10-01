using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Konteh.Infrastructure.Configuration;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder
            .HasMany(eq => eq.ExamQuestions)
            .WithOne();

        builder.HasOne(x => x.Candidate);
    }
}
