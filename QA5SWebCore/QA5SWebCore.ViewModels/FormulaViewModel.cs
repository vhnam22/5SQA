using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class FormulaViewModel : AuditableEntity
{
	public Guid? MeasurementId { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public string ValueOld { get; set; }

	public string ValueOrigin { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? UpperOld { get; set; }

	public double? UpperOrigin { get; set; }

	public double? Lower { get; set; }

	public double? LowerOld { get; set; }

	public double? LowerOrigin { get; set; }

	public double? WarnUpper { get; set; }

	public double? WarnLower { get; set; }

	public string Formula { get; set; }

	public string Result { get; set; }

	public string ResultOld { get; set; }

	public string ResultOrigin { get; set; }

	public string MachineName { get; set; }

	public string MachineNameOld { get; set; }

	public string StaffName { get; set; }

	public string MachineTypeName { get; set; }

	public string History { get; set; }

	public int? Sample { get; set; }

	public int? Cavity { get; set; }

	public bool IsChange { get; set; }

	public bool IsChangeOther { get; set; }

	public bool IsChangeByFormula { get; set; }
}
