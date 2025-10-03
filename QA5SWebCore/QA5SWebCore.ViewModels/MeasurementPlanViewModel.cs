using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class MeasurementPlanViewModel : AuditableEntity
{
	public bool? IsSelect { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public Guid? ImportantId { get; set; }

	public string ImportantName { get; set; }

	public string Value { get; set; }

	public Guid? UnitId { get; set; }

	public string UnitName { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public Guid? MachineTypeId { get; set; }

	public string MachineTypeName { get; set; }

	public string Formula { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public int? Sort { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductName { get; set; }
}
