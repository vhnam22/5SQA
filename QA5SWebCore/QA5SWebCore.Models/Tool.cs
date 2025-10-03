using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Tool : AuditableEntity
{
	public Guid? TabletId { get; set; }

	public Guid? MachineId { get; set; }

	public virtual Machine Tablet { get; set; }

	public virtual Machine Machine { get; set; }
}
