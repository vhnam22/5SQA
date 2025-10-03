using System;

namespace QA5SWebCore.Utilities.Dtos;

public class ResultForChartDto
{
	public Guid ProductId { get; set; }

	public DateTimeOffset Date { get; set; }

	public int Limit { get; set; }
}
