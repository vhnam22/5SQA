using System;

namespace QA5SWebCore.Data.Interfaces;

public interface IEntity
{
	Guid Id { get; set; }

	DateTimeOffset Created { get; set; }

	DateTimeOffset Modified { get; set; }

	bool IsActivated { get; set; }
}
