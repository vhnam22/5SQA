using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class ProductGroupViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Version { get; set; }

	public DateTimeOffset? ReleaseDate { get; set; }

	public string RevisionHistory { get; set; }

	public string PreparedBy { get; set; }

	public string CheckedBy { get; set; }

	public string ApprovedBy { get; set; }

	public int? ProductCount { get; set; }
}
