using Jantzch.Server2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Jantzch.Server2.Infraestructure;

public class JantzchContext: DbContext
{
    public JantzchContext(DbContextOptions<JantzchContext> options) : base(options) { }

    public DbSet<Material> Materials { get; set; } = null!;

    public DbSet<GroupMaterial> GroupMaterials { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Value).IsRequired();
            entity.Property(e => e.Eu).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired();           
        });

        modelBuilder.Entity<GroupMaterial>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
        });
    }
}
