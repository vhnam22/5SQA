using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class TemplateViewModel : AuditableEntity
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int? Limit { get; set; }

	public string Type { get; set; }

	public string TemplateUrl { get; set; }

	public string TemplateData { get; set; }
}
