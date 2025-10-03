using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Calibration : AuditableEntity
{
	public string CalibrationNo { get; set; }

	public Guid? TypeId { get; set; }

	public virtual MetadataValue Type { get; set; }

	public string File { get; set; }

	public Guid? MachineId { get; set; }

	public string Company { get; set; }

	public string Staff { get; set; }

	public DateTimeOffset? CalDate { get; set; }

	public DateTimeOffset? ExpDate { get; set; }

	public int? Period { get; set; }

	public virtual Machine Machine { get; set; }
}
