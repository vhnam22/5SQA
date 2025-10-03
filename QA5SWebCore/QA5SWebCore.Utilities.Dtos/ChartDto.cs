using System;

namespace QA5SWebCore.Utilities.Dtos;

public class ChartDto
{
	public Guid ProductId { get; set; }

	public DateTimeOffset StartDate { get; set; }

	public DateTimeOffset EndDate { get; set; }

	public string Mold { get; set; }

	public Guid MeasurementId { get; set; }

	public Guid ProductGroupId { get; set; }
}
