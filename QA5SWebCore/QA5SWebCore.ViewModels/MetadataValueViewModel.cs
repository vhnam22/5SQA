using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class MetadataValueViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Value { get; set; }

	public Guid? TypeId { get; set; }

	public string TypeCode { get; set; }

	public string TypeName { get; set; }

	public Guid? ParentId { get; set; }

	public string ParentCode { get; set; }

	public string ParentName { get; set; }
}
