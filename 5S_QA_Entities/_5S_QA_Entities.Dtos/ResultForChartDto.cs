using System;

namespace _5S_QA_Entities.Dtos;

public class ResultForChartDto
{
	public Guid? ProductId { get; set; }

	public DateTimeOffset? Date { get; set; }

	public int Limit { get; set; }
}
