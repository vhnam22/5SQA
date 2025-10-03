using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class MeasurementViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public Guid? ImportantId { get; set; }

	public string ImportantName { get; set; }

	public string Value { get; set; }

	public Guid? UnitId { get; set; }

	public string UnitName { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public double? UCL { get; set; }

	public double? LCL { get; set; }

	public string Formula { get; set; }

	public Guid? MachineTypeId { get; set; }

	public string MachineTypeName { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public Guid? ProductId { get; set; }

	public string Coordinate { get; set; }
}
