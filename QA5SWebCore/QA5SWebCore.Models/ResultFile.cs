using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class ResultFile : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public virtual Request Request { get; set; }

	public string Link { get; set; }
}
