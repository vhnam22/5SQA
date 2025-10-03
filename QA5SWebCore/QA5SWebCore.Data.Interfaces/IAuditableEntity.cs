namespace QA5SWebCore.Data.Interfaces;

public interface IAuditableEntity : IEntity
{
	string CreatedBy { get; set; }

	string ModifiedBy { get; set; }
}
