using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class ProductGroup : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Description { get; set; }

	public string Version { get; set; }

	public DateTimeOffset? ReleaseDate { get; set; }

	public string RevisionHistory { get; set; }

	public string PreparedBy { get; set; }

	public string CheckedBy { get; set; }

	public string ApprovedBy { get; set; }

	public virtual ICollection<Product> Products { get; set; }

	public ProductGroup()
	{
		Products = new HashSet<Product>();
	}
}
