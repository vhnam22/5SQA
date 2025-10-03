using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Template : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public string Description { get; set; }

	public int Limit { get; set; }

	[Required]
	public string Type { get; set; }

	public string TemplateUrl { get; set; }

	public string TemplateData { get; set; }
}
