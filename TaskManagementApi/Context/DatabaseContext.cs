
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Reflection;
using TaskManagementApi.Contracts;
using TaskManagementApi.Models;

namespace CoreApi.Persistence.Context;

public class DatabaseContext : DbContext
{
    private readonly ICurrentDateProvider _currentDateProvider;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, ICurrentDateProvider currentDateProvider) :
      base(options)
    {
        _currentDateProvider = currentDateProvider;
        Options = options;
    }

  public DbContextOptions<DatabaseContext> Options { get; }
  public DbSet<TaskData> Tasks { get; set; }
  public DbSet<User> Users { get; set; }
  

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
   
  }

  public override int SaveChanges(bool acceptAllChangesOnSuccess)
  {
        SetEntitiesAuditInfo();
        TryUpdateEntitiesVersion();

        return base.SaveChanges(acceptAllChangesOnSuccess);
  }

 public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
      CancellationToken cancellationToken = new())
  {
        SetEntitiesAuditInfo();
        TryUpdateEntitiesVersion();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
  }

 private void SetEntitiesAuditInfo()
  {
        SetEntitiesCreatedOnSave();
        SetEntitiesUpdatedOnSave();
  }

    private void SetEntitiesCreatedOnSave()
    {
        var entitiesToCreate = FilterTrackedEntriesByState(EntityState.Added);

        foreach (var entity in entitiesToCreate) entity.CreatedOn = _currentDateProvider.Now;
    }

    private void SetEntitiesUpdatedOnSave()
    {
        var entitiesToUpdate = FilterTrackedEntriesByState(EntityState.Modified);

        foreach (var entity in entitiesToUpdate) entity.LastModifiedOn = _currentDateProvider.Now;
    }

    private void TryUpdateEntitiesVersion()
    {
        var entries = ChangeTracker
          .Entries()
          .Where(entry => entry.Entity is IBaseEntity && entry.State is EntityState.Modified);

        foreach (var entry in entries)
            if (entry.Entity is BaseVersionedEntity versionedEntity)
            {
                var shouldIncreaseVersion = entry.Properties.Any(prop =>
                  prop.IsModified &&
                  versionedEntity.VersionedFields.Contains(prop.Metadata.Name));

                if (shouldIncreaseVersion) versionedEntity.Version++;
            }
    }

  private IEnumerable<IBaseEntity> FilterTrackedEntriesByState(EntityState entityState)
  {
    return ChangeTracker
      .Entries()
      .Where(e => e.Entity is IBaseEntity && e.State == entityState)
      .Select(e => (IBaseEntity)e.Entity);
  }
  
}
