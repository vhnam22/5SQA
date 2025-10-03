using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class PlanDetailViewModel : AuditableEntity
{
	public Guid? PlanId { get; set; }

	public string PlanName { get; set; }

	public Guid? MeasurementId { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string ImportantName { get; set; }

	public string Value { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string UnitName { get; set; }

	public string MachineTypeName { get; set; }

	public string Formula { get; set; }

	public string TemplateCode { get; set; }

	public string TemplateName { get; set; }

	public int? Sort { get; set; }
}
