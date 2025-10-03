using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class MetadataValueViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Value { get; set; }

	public Guid? ParentId { get; set; }

	public string ParentCode { get; set; }

	public string ParentName { get; set; }

	public Guid TypeId { get; set; }

	public string TypeCode { get; set; }

	public string TypeName { get; set; }
}
