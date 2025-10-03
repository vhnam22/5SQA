using System;

namespace _5S_QA_Entities.Models;

public class MeasurementStatisticViewModel
{
	public Guid? Id { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductName { get; set; }
}
