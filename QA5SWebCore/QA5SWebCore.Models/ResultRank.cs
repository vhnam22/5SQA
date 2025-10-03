using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class ResultRank : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public virtual Request Request { get; set; }

	public Guid? MeasurementId { get; set; }

	public virtual Measurement Measurement { get; set; }

	public string Rank { get; set; }

	public int? Percentage { get; set; }

	public int Sample { get; set; }
}
