using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class ProductViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string CodeName { get; set; }

	public string Description { get; set; }

	public string ImageUrl { get; set; }

	public int? Cavity { get; set; }

	public int? SampleMax { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public Guid? DepartmentId { get; set; }

	public string DepartmentName { get; set; }

	public int? TotalMeas { get; set; }

	public bool? IsAQL { get; set; }

	public int? Sort { get; set; }

	public Guid? GroupId { get; set; }

	public string GroupCode { get; set; }

	public string GroupName { get; set; }

	public string GroupCodeName { get; set; }
}
