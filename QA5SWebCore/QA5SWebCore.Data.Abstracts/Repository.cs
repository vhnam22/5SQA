using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Utilities.Helppers;

namespace QA5SWebCore.Data.Abstracts;

public class Repository<T> : IRepository<T> where T : class, IEntity, new()
{
	protected readonly DbContext _context;

	protected readonly IActionContextAccessor _ctxAccessor;

	private readonly bool _forceAllItems;

	private IQueryable<T> Query => _context.Set<T>().AsQueryable();

	private string ServiceOwner
	{
		get
		{
			try
			{
				if (_ctxAccessor.ActionContext.HttpContext.User == null)
				{
					return null;
				}
				return (from x in _ctxAccessor.ActionContext.HttpContext.User.Claims
					where x.Type == "client_id"
					select x.Value).FirstOrDefault();
			}
			catch (Exception)
			{
				return null;
			}
		}
	}

	private string IdentityName
	{
		get
		{
			try
			{
				if (_ctxAccessor.ActionContext.HttpContext.User != null)
				{
					return _ctxAccessor.ActionContext.HttpContext.User.Identity.Name;
				}
				throw new Exception();
			}
			catch
			{
				return null;
			}
		}
	}

	public Repository(DbContext context, IActionContextAccessor ctxAccessor, bool forceAllItems)
	{
		_context = context;
		_ctxAccessor = ctxAccessor;
		_forceAllItems = forceAllItems;
	}

	public IEnumerable<T> GetAll(string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.OrderBy(order).AsEnumerable();
	}

	public IEnumerable<T> GetAll(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.AsEnumerable();
	}

	public async Task<IEnumerable<T>> GetAllAsync(string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.OrderBy(order).ToListAsync();
	}

	public async Task<IEnumerable<T>> GetAllAsync(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ToListAsync();
	}

	public async Task<IEnumerable<T>> GetAllAsync<TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ToListAsync();
	}

	[Obsolete]
	public void SetAllAsync(string query)
	{
		_context.Database.ExecuteSqlRaw(query);
	}

	public T FindById(Guid id, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.FirstOrDefault((T x) => x.Id == id);
	}

	public T FindById(Guid id)
	{
		IQueryable<T> query = Query;
		return query.FirstOrDefault((T x) => x.Id == id);
	}

	public async Task<T> FindByIdAsync(Guid id)
	{
		IQueryable<T> query = Query;
		return await query.FirstOrDefaultAsync((T x) => x.Id == id);
	}

	public async Task<T> FindByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.FirstOrDefaultAsync((T e) => e.Id == id);
	}

	public T GetSingle(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		queryable = queryable.OrderBy(order);
		if (predicate == null)
		{
			return queryable.FirstOrDefault();
		}
		return queryable.Where(predicate).FirstOrDefault();
	}

	public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		queryable = queryable.OrderBy(order);
		if (predicate == null)
		{
			return await queryable.FirstOrDefaultAsync();
		}
		return await queryable.Where(predicate).FirstOrDefaultAsync();
	}

	public async Task<T> GetSingleAsync(string predicate, object[] parameters, string order = "")
	{
		return await Query.Where(predicate, parameters).OrderBy(order).FirstOrDefaultAsync();
	}

	public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.Where(predicate).OrderBy(order).AsEnumerable();
	}

	public IEnumerable<T> FindBy(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.AsEnumerable();
	}

	public IEnumerable<T> FindBy(string predicate, object[] parameters, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.Where(predicate, parameters).OrderBy(order).AsEnumerable();
	}

	public IEnumerable<T> FindBy(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return queryable.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.AsEnumerable();
	}

	public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.Where(predicate).OrderBy(order).ToListAsync();
	}

	public async Task<IEnumerable<T>> FindByAsync(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ToListAsync();
	}

	public async Task<IEnumerable<T>> FindByAsync(string predicate, object[] parameters, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.Where(predicate, parameters).OrderBy(order).ToListAsync();
	}

	public async Task<IEnumerable<T>> FindByAsync(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ToListAsync();
	}

	public IEnumerable<H> GetAll<H>(string order = "")
	{
		return Query.OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>()).AsEnumerable();
	}

	public IEnumerable<H> GetAll<H>(string order, int pageIndex, int limit)
	{
		return Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.AsEnumerable();
	}

	public async Task<IEnumerable<H>> GetAllAsync<H>(string order = "")
	{
		return await Query.OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>()).ToListAsync();
	}

	public async Task<IEnumerable<H>> GetAllAsync<H>(string order, int pageIndex, int limit)
	{
		return await Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<H>> GetAllAsync<H, TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit)
	{
		return await Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public H FindById<H>(Guid id)
	{
		return Query.Where((T x) => x.Id == id).ProjectTo(Array.Empty<Expression<Func<H, object>>>()).FirstOrDefault();
	}

	public async Task<H> FindByIdAsync<H>(Guid id)
	{
		return await Query.Where((T x) => x.Id == id).ProjectTo(Array.Empty<Expression<Func<H, object>>>()).FirstOrDefaultAsync();
	}

	public H GetSingle<H>(Expression<Func<T, bool>> predicate, string order = "")
	{
		if (predicate == null)
		{
			return Query.OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>()).FirstOrDefault();
		}
		return Query.Where(predicate).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.FirstOrDefault();
	}

	public async Task<H> GetSingleAsync<H>(Expression<Func<T, bool>> predicate, string order = "")
	{
		_ = Query;
		return await Query.Where(predicate).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.FirstOrDefaultAsync();
	}

	public IEnumerable<H> FindBy<H>(Expression<Func<T, bool>> predicate, string order = "")
	{
		return Query.Where(predicate).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.AsEnumerable();
	}

	public IEnumerable<H> FindBy<H>(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
	{
		return Query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.AsEnumerable();
	}

	public IEnumerable<H> FindBy<H>(string predicate, object[] parameters, string order = "")
	{
		return Query.Where(predicate, parameters).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.AsEnumerable();
	}

	public IEnumerable<H> FindBy<H>(string order, int pageIndex, int limit, string predicate, object[] parameters)
	{
		return Query.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.AsEnumerable();
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(string predicate, object[] parameters, string order = "")
	{
		return await Query.Where(predicate, parameters).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, string predicate, object[] parameters)
	{
		return await Query.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(Expression<Func<T, bool>> predicate, string order = "")
	{
		return await Query.Where(predicate).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
	{
		return await Query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<H>> FindByAsync<H, TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
	{
		return await Query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit)
			.Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public IQueryable<T> GetQuery()
	{
		return Query;
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(IQueryable<T> query, string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
	{
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			query = query.Include(selector);
		}
		return await query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}

	public async Task<IEnumerable<T>> FindByAsync(IQueryable<T> query, string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
	{
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			query = query.Include(selector);
		}
		return await query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit)
			.ToListAsync();
	}

	public int CountAll()
	{
		return Query.Count();
	}

	public async Task<int> CountAllAsync()
	{
		return await Query.CountAsync();
	}

	public int Count(Expression<Func<T, bool>> predicate)
	{
		return Query.Where(predicate).Count();
	}

	public int Count(string predicate, object[] parameters)
	{
		return Query.Where(predicate, parameters).Count();
	}

	public bool Any(Expression<Func<T, bool>> predicate)
	{
		return Query.Where(predicate).Any();
	}

	public bool Any(string predicate, object[] parameters)
	{
		return Query.Where(predicate, parameters).Any();
	}

	public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
	{
		return await Query.Where(predicate).AnyAsync();
	}

	public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
	{
		return await Query.Where(predicate).CountAsync();
	}

	public async Task<bool> AnyAsync(string predicate, object[] parameters)
	{
		return await Query.Where(predicate, parameters).AnyAsync();
	}

	public async Task<int> CountAsync(string predicate, object[] parameters)
	{
		return await Query.Where(predicate, parameters).CountAsync();
	}

	public async Task<int> CountAsync(IQueryable<T> query)
	{
		return await query.CountAsync();
	}

	public void Detach(T entity)
	{
		_context.Entry(entity).State = EntityState.Detached;
	}

	public void Detach(IEnumerable<T> entities)
	{
		foreach (T entity in entities)
		{
			_context.Entry(entity).State = EntityState.Detached;
		}
	}

	public void Add(T entity, bool isActive = true)
	{
		entity.Id = Guid.NewGuid();
		entity.IsActivated = isActive;
		if (entity is IAuditableEntity auditableEntity)
		{
			auditableEntity.CreatedBy = IdentityName ?? "";
			auditableEntity.ModifiedBy = IdentityName ?? "";
		}
		_context.Set<T>().Add(entity);
	}

	public void Update(T entity)
	{
		EntityEntry<T> entityEntry = _context.Entry(entity);
		entityEntry.State = EntityState.Modified;
		entityEntry.Property((T x) => x.Created).IsModified = false;
		if (entity is IAuditableEntity auditableEntity)
		{
			auditableEntity.ModifiedBy = IdentityName;
			EntityEntry<IAuditableEntity> entityEntry2 = _context.Entry(auditableEntity);
			entityEntry2.Property((IAuditableEntity x) => x.CreatedBy).IsModified = false;
		}
	}

	public void Delete(T entity)
	{
		EntityEntry<T> entityEntry = _context.Entry(entity);
		entityEntry.State = EntityState.Deleted;
	}

	public void Add(IEnumerable<T> entities, bool isActive = true)
	{
		foreach (T entity in entities)
		{
			entity.Id = Guid.NewGuid();
			entity.IsActivated = isActive;
			if (entity is IAuditableEntity auditableEntity)
			{
				auditableEntity.CreatedBy = IdentityName;
				auditableEntity.ModifiedBy = IdentityName;
			}
		}
		_context.Set<T>().AddRange(entities);
	}

	public void AddNoIdentity(IEnumerable<T> entities, bool isActive = true)
	{
		foreach (T entity in entities)
		{
			entity.Id = Guid.NewGuid();
			entity.IsActivated = isActive;
		}
		_context.Set<T>().AddRange(entities);
	}

	public void Update(IEnumerable<T> entities, bool isActive = true)
	{
		foreach (T entity in entities)
		{
			Update(entity);
		}
	}

	public void Delete(IEnumerable<T> entities)
	{
		foreach (T entity in entities)
		{
			Delete(entity);
		}
	}

	public int Commit()
	{
		EnsureAutoHistory();
		return _context.SaveChanges();
	}

	public async Task<int> CommitAsync()
	{
		EnsureAutoHistory();
		return await _context.SaveChangesAsync();
	}

	public void Load(T entity, params Expression<Func<T, object>>[] includeProperties)
	{
		foreach (Expression<Func<T, object>> propertyExpression in includeProperties)
		{
			_context.Entry(entity).Reference(propertyExpression).Load();
		}
	}

	private void EnsureAutoHistory()
	{
	}

	public async Task<IEnumerable<H>> FindByAsync<H>(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
	{
		IQueryable<T> queryable = Query;
		foreach (Expression<Func<T, object>> selector in includeProperties)
		{
			queryable = queryable.Include(selector);
		}
		return await queryable.Where(predicate).OrderBy(order).ProjectTo(Array.Empty<Expression<Func<H, object>>>())
			.ToListAsync();
	}
}
