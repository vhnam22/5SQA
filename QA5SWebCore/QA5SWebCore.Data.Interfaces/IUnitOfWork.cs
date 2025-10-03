using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QA5SWebCore.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
	IRepository<T> GetRepository<T>() where T : class, IEntity, new();

	int Commit();

	Task<int> CommitAsync();
}
public interface IUnitOfWork<TContext> : IUnitOfWork, IDisposable where TContext : DbContext
{
	TContext Context { get; }
}
