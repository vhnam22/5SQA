using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QA5SWebCore.Data.Abstracts;
using QA5SWebCore.Data.Interfaces;

namespace QA5SWebCore.Data;

public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork, IDisposable where TContext : DbContext
{
	private Dictionary<Type, object> _repositories;

	protected readonly IActionContextAccessor _ctxAccessor;

	private IConfigurationProvider _mapperConfiguration;


    protected TContext Context { get; }

	TContext IUnitOfWork<TContext>.Context
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public UnitOfWork(TContext context, IActionContextAccessor ctxAccessor, IConfigurationProvider mapperConfiguration)
	{
		Context = context ?? throw new ArgumentNullException("context");
		_ctxAccessor = ctxAccessor;
		_mapperConfiguration= mapperConfiguration;
	}

	public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity, new()
	{
		if (_repositories == null)
		{
			_repositories = new Dictionary<Type, object>();
		}
		Type typeFromHandle = typeof(TEntity);
		if (!_repositories.ContainsKey(typeFromHandle))
		{
			_repositories[typeFromHandle] = new Repository<TEntity>(Context, _ctxAccessor, forceAllItems: false, _mapperConfiguration);
		}
		return (IRepository<TEntity>)_repositories[typeFromHandle];
	}

	public int Commit()
	{
		return Context.SaveChanges();
	}

	public async Task<int> CommitAsync()
	{
		return await Context.SaveChangesAsync();
	}

	public void Dispose()
	{
		Context?.Dispose();
	}
}
