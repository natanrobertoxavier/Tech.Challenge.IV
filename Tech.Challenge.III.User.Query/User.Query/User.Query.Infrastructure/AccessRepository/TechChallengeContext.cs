using Microsoft.EntityFrameworkCore;

namespace User.Query.Infrastructure.AccessRepository;
public class TechChallengeContext(
    DbContextOptions<TechChallengeContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeContext).Assembly);
    }
}
