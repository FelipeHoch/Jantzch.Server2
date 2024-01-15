﻿using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
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