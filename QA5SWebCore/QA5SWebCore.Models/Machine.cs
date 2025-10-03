using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Machine : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Model { get; set; }

	public string Serial { get; set; }

	public string Status { get; set; }

	public Guid? FactoryId { get; set; }

	public virtual MetadataValue Factory { get; set; }

	public Guid MachineTypeId { get; set; }

	public virtual MetadataValue MachineType { get; set; }

	public int? Mark { get; set; }

	public virtual ICollection<Calibration> Calibrations { get; set; }

	public Machine()
	{
		Calibrations = new HashSet<Calibration>();
	}
}
