using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class RequestStatistic : AuditableEntity
{
	public Guid RequestId { get; set; }

	public virtual Request Request { get; set; }

	public int MeasAll { get; set; }

	public int ResultNG { get; set; }

	public int ResultAll { get; set; }

	public int SampleNG { get; set; }
}
