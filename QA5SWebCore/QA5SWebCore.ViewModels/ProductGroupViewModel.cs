using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class ProductGroupViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string CodeName { get; set; }

	public string Description { get; set; }

	public string Version { get; set; }

	public DateTimeOffset? ReleaseDate { get; set; }

	public string RevisionHistory { get; set; }

	public string PreparedBy { get; set; }

	public string CheckedBy { get; set; }

	public string ApprovedBy { get; set; }

	public int? ProductCount { get; set; }
}
