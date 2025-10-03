using System;

namespace QA5SWebCore.Utilities.Dtos;

public class StatisticDetailDto
{
	public DateTime Date { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MeasurementName { get; set; }

	public string Type { get; set; }
}
