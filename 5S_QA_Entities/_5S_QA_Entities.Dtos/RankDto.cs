using System;

namespace _5S_QA_Entities.Dtos;

public class RankDto
{
	public Guid? MeasurementId { get; set; }

	public string MeasurementName { get; set; }

	public Guid? RequestId { get; set; }

	public string Rank { get; set; }

	public int? Percentage { get; set; }

	public int? Sample { get; set; }

	public int? Total { get; set; }
}
