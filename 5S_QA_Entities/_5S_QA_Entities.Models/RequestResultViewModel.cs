using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class RequestResultViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MachineTypeName { get; set; }

	public string MeasurementCode { get; set; }

	public string MeasurementUnit { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public double? LSL { get; set; }

	public double? USL { get; set; }

	public string Formula { get; set; }

	public int? Cavity { get; set; }

	public string Result { get; set; }

	public string Judge { get; set; }

	public string MachineName { get; set; }

	public string StaffName { get; set; }

	public string History { get; set; }

	public int? Sample { get; set; }

	public RequestResultViewModel Clone()
	{
		return (RequestResultViewModel)MemberwiseClone();
	}
}
