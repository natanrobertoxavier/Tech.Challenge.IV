using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Persistence.Domain.Entities;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess;
public class TechChallengeContext(DbContextOptions<TechChallengeContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RegionDDD> DDDRegions { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeContext).Assembly);
    }
}