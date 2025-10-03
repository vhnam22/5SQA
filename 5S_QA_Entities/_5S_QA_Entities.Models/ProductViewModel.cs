using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class ProductViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string ImageUrl { get; set; }

	public int? Cavity { get; set; }

	public int? SampleMax { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public Guid? DepartmentId { get; set; }

	public string DepartmentName { get; set; }

	public int? TotalMeas { get; set; }

	public int? IsAQL { get; set; }

	public Guid? GroupId { get; set; }

	public string GroupCodeName { get; set; }
}
