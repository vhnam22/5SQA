using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class PlanViewModel : AuditableEntity
{
	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public Guid? StageId { get; set; }

	public string StageName { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public int? TotalDetails { get; set; }
}
