using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class ChartViewModel : AuditableEntity
{
	public bool? IsSelect { get; set; }

	public Guid? RequestId { get; set; }

	public string RequestName { get; set; }

	public DateTimeOffset? RequestDate { get; set; }

	public string RequestSample { get; set; }

	public string Lot { get; set; }

	public string Line { get; set; }

	public Guid? ProductId { get; set; }

	public Guid? MeasurementId { get; set; }

	public string MeasurementCode { get; set; }

	public string MachineTypeName { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public string Unit { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public double? LSL { get; set; }

	public double? USL { get; set; }

	public double? LCL { get; set; }

	public double? UCL { get; set; }

	public int? Sample { get; set; }

	public int? Cavity { get; set; }

	public string Result { get; set; }

	public string Judge { get; set; }

	public string MachineName { get; set; }

	public string StaffName { get; set; }

	public string History { get; set; }
}
