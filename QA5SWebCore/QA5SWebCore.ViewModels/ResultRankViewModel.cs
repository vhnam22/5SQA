using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class ResultRankViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MeasurementName { get; set; }

	public string Rank { get; set; }

	public int? Percentage { get; set; }

	public int? Sample { get; set; }

	public int? Total { get; set; }
}
