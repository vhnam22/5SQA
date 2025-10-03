using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class MetadataValue : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Description { get; set; }

	public string Value { get; set; }

	public Guid TypeId { get; set; }

	public Guid? ParentId { get; set; }

	public virtual MetadataType Type { get; set; }

	public virtual MetadataValue Parent { get; set; }
}
