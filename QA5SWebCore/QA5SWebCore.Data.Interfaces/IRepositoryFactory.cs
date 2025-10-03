namespace QA5SWebCore.Data.Interfaces;

public interface IRepositoryFactory
{
	IRepository<T> GetRepository<T>() where T : class, IEntity, new();
}
