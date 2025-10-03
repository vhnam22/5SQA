using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class MetadataType : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Description { get; set; }

	public Guid? ParentId { get; set; }

	public virtual MetadataType Parent { get; set; }
}
