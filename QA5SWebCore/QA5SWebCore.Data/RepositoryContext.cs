using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Enums;

namespace QA5SWebCore.Data;

public class RepositoryContext : DbContext
{
	public DbSet<MetadataType> MetadataTypes { get; set; }

	public DbSet<MetadataValue> MetadataValues { get; set; }

	public DbSet<AuthUser> AuthUsers { get; set; }

	public DbSet<Machine> Machines { get; set; }

	public DbSet<Product> Products { get; set; }

	public DbSet<Measurement> Measurements { get; set; }

	public DbSet<Plan> Plans { get; set; }

	public DbSet<PlanDetail> PlanDetails { get; set; }

	public DbSet<Template> Templates { get; set; }

	public DbSet<Tool> Tools { get; set; }

	public DbSet<Request> Requests { get; set; }

	public DbSet<RequestStatus> RequestStatuss { get; set; }

	public DbSet<RequestStatistic> RequestStatistics { get; set; }

	public DbSet<RequestResultStatistic> RequestResultStatistics { get; set; }

	public DbSet<RequestResult> RequestResults { get; set; }

	public DbSet<Comment> Comments { get; set; }

	public DbSet<Constant> Constants { get; set; }

	public DbSet<Email> Emails { get; set; }

	public DbSet<Similar> Similars { get; set; }

	public DbSet<TemplateOther> TemplateOthers { get; set; }

	public DbSet<Calibration> Calibrations { get; set; }

	public DbSet<ResultFile> ResultFiles { get; set; }

	public DbSet<Setting> Settings { get; set; }

	public DbSet<AQL> AQLs { get; set; }

	public DbSet<ResultRank> ResultRanks { get; set; }

	public DbSet<ProductGroup> GroupProducts { get; set; }

	public RepositoryContext()
	{
	}

	public RepositoryContext(DbContextOptions<RepositoryContext> options)
		: base(options)
	{
	}

	public RepositoryContext(string connectionString)
		: base(GetOptions(connectionString))
	{
	}

	private static DbContextOptions GetOptions(string connectionString)
	{
		return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
	}

	public override int SaveChanges()
	{
		IEnumerable<EntityEntry> enumerable = from e in ChangeTracker.Entries()
			where e.State == EntityState.Modified || e.State == EntityState.Added
			select e;
		foreach (EntityEntry item in enumerable)
		{
			if (item.Entity is IEntity entity)
			{
				if (item.State == EntityState.Added)
				{
					entity.Created = DateTimeOffset.Now;
				}
				entity.Modified = DateTimeOffset.Now;
			}
		}
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IEnumerable<EntityEntry> enumerable = from e in ChangeTracker.Entries()
			where e.State == EntityState.Modified || e.State == EntityState.Added
			select e;
		foreach (EntityEntry item in enumerable)
		{
			if (item.Entity is IEntity entity)
			{
				if (item.State == EntityState.Added)
				{
					entity.Created = DateTimeOffset.Now;
				}
				entity.Modified = DateTimeOffset.Now;
			}
		}
		return await base.SaveChangesAsync();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AuthUser>().Property((AuthUser b) => b.Role).HasDefaultValue(RoleWeb.Member);
		base.OnModelCreating(modelBuilder);
	}
}
