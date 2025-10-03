using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Measurement : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public Guid? ImportantId { get; set; }

	public virtual MetadataValue Important { get; set; }

	[Required]
	public string Value { get; set; }

	public double? Upper { get; set; }

	public double? Lower { get; set; }

	public double? UCL { get; set; }

	public double? LCL { get; set; }

	public Guid? UnitId { get; set; }

	public virtual MetadataValue Unit { get; set; }

	public Guid MachineTypeId { get; set; }

	public virtual MetadataValue MachineType { get; set; }

	public string Formula { get; set; }

	public Guid? TemplateId { get; set; }

	public virtual Template Template { get; set; }

	public string Coordinate { get; set; }

	public int Sort { get; set; }

	public Guid ProductId { get; set; }

	public virtual Product Product { get; set; }
}
