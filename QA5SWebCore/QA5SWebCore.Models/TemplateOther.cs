using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class TemplateOther : AuditableEntity
{
	public Guid ProductId { get; set; }

	public virtual Product Product { get; set; }

	public Guid TemplateId { get; set; }

	public virtual Template Template { get; set; }
}
