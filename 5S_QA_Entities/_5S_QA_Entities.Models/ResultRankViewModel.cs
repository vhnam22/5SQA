using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class ResultRankViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public Guid? MeasurementId { get; set; }

	public string Rank { get; set; }

	public int? Percentage { get; set; }

	public int? Sample { get; set; }
}
