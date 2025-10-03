using System;

namespace _5S_QA_Entities.Models;

public class MeasurementPlanViewModel
{
	public bool? IsSelect { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string ImportantName { get; set; }

	public string Value { get; set; }

	public string UnitName { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string MachineTypeName { get; set; }

	public Guid? Id { get; set; }
}
