using Microsoft.EntityFrameworkCore;
using Region.Query.Domain.Entities;

namespace Region.Query.Infrastructure.RepositoryAccess;
public class TechChallengeContext(DbContextOptions<TechChallengeContext> options) : DbContext(options)
{
    public DbSet<RegionDDD> DDDRegions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeContext).Assembly);
    }
}