using System;
using QA5SWebCore.Data.Interfaces;

namespace QA5SWebCore.Data.Abstracts;

public abstract class Entity : IEntity
{
	public Guid Id { get; set; }

	public DateTimeOffset Created { get; set; }

	public DateTimeOffset Modified { get; set; }

	public bool IsActivated { get; set; }
}
