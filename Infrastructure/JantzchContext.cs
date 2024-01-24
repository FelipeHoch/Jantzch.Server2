using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Jantzch.Server2.Infraestructure;

public class JantzchContext: DbContext
{
    private IDbContextTransaction? _currentTransaction;

    public JantzchContext(DbContextOptions<JantzchContext> options) : base(options) { }

    public DbSet<Material> Materials { get; set; } = null!;

    public DbSet<GroupMaterial> GroupMaterials { get; set; } = null!;

    public DbSet<Tax> Taxes { get; set; } = null!;

    public DbSet<ReportConfiguration> ReportConfiguration { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var materialEntity = modelBuilder.Entity<Material>().ToCollection("materials");

        materialEntity.Property(m => m.Id).HasElementName("_id");
        materialEntity.Property(m => m.Name).HasElementName("name");
        materialEntity.Property(m => m.Value).HasElementName("value");
        materialEntity.Property(m => m.Eu).HasElementName("eu");
        materialEntity.Property(m => m.CreatedBy).HasElementName("createdBy");
        materialEntity.Property(m => m.GroupIdObject).HasElementName("groupId");


        var groupMaterialEntity = modelBuilder.Entity<GroupMaterial>().ToCollection("groups_material");

        groupMaterialEntity.Property(g => g.Id).HasElementName("_id");
        groupMaterialEntity.Property(g => g.Name).HasElementName("name");
        groupMaterialEntity.Property(g => g.Description).HasElementName("description");

        var taxEntity = modelBuilder.Entity<Tax>().ToCollection("taxes");

        taxEntity.Property(t => t.Id).HasElementName("_id");
        taxEntity.Property(t => t.Name).HasElementName("name");
        taxEntity.Property(t => t.Type).HasElementName("type");
        taxEntity.Property(t => t.CreatedBy).HasElementName("createdBy");
        taxEntity.Property(t => t.Value).HasElementName("value");
        taxEntity.Property(t => t.Code).HasElementName("code");

        var reportConfigurationEntity = modelBuilder.Entity<ReportConfiguration>().ToCollection("report_configuration");

        reportConfigurationEntity.Property(r => r.Id).HasElementName("_id");
        reportConfigurationEntity.Property(r => r.ReportKey).HasElementName("reportKey");
        reportConfigurationEntity.Property(r => r.BottomTitle).HasElementName("bottomTitle");
        reportConfigurationEntity.Property(r => r.BottomText).HasElementName("bottomText");
        reportConfigurationEntity.Property(r => r.PhoneContact).HasElementName("phoneContact");
        reportConfigurationEntity.Property(r => r.EmailContact).HasElementName("emailContact");
        reportConfigurationEntity.Property(r => r.SiteUrl).HasElementName("siteUrl");

        var userEntity = modelBuilder.Entity<User>().ToCollection("users");

        userEntity.Property(u => u.Id).HasElementName("_id");
        userEntity.Property(u => u.IdentityProviderId).HasElementName("identityProviderId");
        userEntity.Property(u => u.Name).HasElementName("name");
        userEntity.Property(u => u.Email).HasElementName("email");
        userEntity.Property(u => u.Provider).HasElementName("provider");
        userEntity.Property(u => u.Role).HasElementName("role");
        userEntity.Property(u => u.CustByHour).HasElementName("custByHour");
    }

    #region Transaction Handling
    public void BeginTransaction()
    {
        if (_currentTransaction != null)
        {
            return;
        }
    }

    public void CommitTransaction()
    {
        try
        {
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    #endregion
}
