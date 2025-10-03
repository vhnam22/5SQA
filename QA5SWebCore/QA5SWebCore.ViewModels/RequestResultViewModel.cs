using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class RequestResultViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public string RequestName { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MeasurementCode { get; set; }

	public string MeasurementName { get; set; }

	public string MeasurementUnit { get; set; }

	public string Name { get; set; }

	public string ImportantName { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public string Formula { get; set; }

	public string MachineTypeName { get; set; }

	public string Result { get; set; }

	public string ResultOrigin { get; set; }

	public string Judge { get; set; }

	public string MachineName { get; set; }

	public string StaffName { get; set; }

	public string History { get; set; }

	public int? Sample { get; set; }

	public int? Cavity { get; set; }

	public int? Sort { get; set; }

	public int? ProductSort { get; set; }
}
