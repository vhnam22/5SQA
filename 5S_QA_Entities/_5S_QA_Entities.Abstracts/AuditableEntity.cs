using System;

namespace _5S_QA_Entities.Abstracts;

public abstract class AuditableEntity
{
	public Guid Id { get; set; }

	public DateTimeOffset Created { get; set; }

	public DateTimeOffset Modified { get; set; }

	public string CreatedBy { get; set; }

	public string ModifiedBy { get; set; }

	public bool IsActivated { get; set; }
}
