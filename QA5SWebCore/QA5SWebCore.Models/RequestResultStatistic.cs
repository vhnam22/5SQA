using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class RequestResultStatistic : AuditableEntity
{
	public Guid RequestId { get; set; }

	public virtual Request Request { get; set; }

	public Guid? MeasurementId { get; set; }

	public virtual Measurement Measurement { get; set; }

	public int NG { get; set; }
}
