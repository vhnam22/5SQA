using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Product : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Description { get; set; }

	public string ImageUrl { get; set; }

	public int Cavity { get; set; }

	public int SampleMax { get; set; }

	public Guid? TemplateId { get; set; }

	public virtual Template Template { get; set; }

	public Guid? DepartmentId { get; set; }

	public virtual MetadataValue Department { get; set; }

	public virtual ICollection<Measurement> Measurements { get; set; }

	public virtual ICollection<AQL> AQLs { get; set; }

	public Guid? GroupId { get; set; }

	public virtual ProductGroup Group { get; set; }

	public int Sort { get; set; }

	public Product()
	{
		Measurements = new HashSet<Measurement>();
		AQLs = new HashSet<AQL>();
	}
}
