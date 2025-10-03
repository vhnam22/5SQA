using System;

namespace _5S_QA_Entities.Models;

public class LimitViewModel
{
	public Guid? MeasurementId { get; set; }

	public string Type { get; set; }

	public int? Sample { get; set; }
}
