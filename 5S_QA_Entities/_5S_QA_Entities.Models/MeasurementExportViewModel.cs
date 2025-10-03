using System;

namespace _5S_QA_Entities.Models;

public class MeasurementExportViewModel
{
	public string Name { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string MachineTypeName { get; set; }

	public DateTimeOffset? DateFrom { get; set; }

	public DateTimeOffset? DateTo { get; set; }
}
