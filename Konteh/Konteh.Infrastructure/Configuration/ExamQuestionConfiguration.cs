using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Konteh.Infrastructure.Configuration;

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder
            .HasOne(eq => eq.Question)
            .WithMany();

        builder
            .HasMany(eq => eq.SelectedAnswers)
            .WithMany();
    }
}
