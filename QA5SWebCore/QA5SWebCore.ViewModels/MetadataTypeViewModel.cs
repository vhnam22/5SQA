using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class MetadataTypeViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid? ParentId { get; set; }

	public string ParentCode { get; set; }

	public string ParentName { get; set; }
}
