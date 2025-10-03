using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class CalibrationViewModel : AuditableEntity
{
	public string CalibrationNo { get; set; }

	public Guid? TypeId { get; set; }

	public string TypeName { get; set; }

	public Guid? MachineId { get; set; }

	public string MachineName { get; set; }

	public string Company { get; set; }

	public string Staff { get; set; }

	public DateTimeOffset? CalDate { get; set; }

	public DateTimeOffset? ExpDate { get; set; }

	public int? Period { get; set; }

	public string File { get; set; }
}
