using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Konteh.Infrastructure;

public class KontehDbContext : DbContext
{
    public KontehDbContext(DbContextOptions<KontehDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
