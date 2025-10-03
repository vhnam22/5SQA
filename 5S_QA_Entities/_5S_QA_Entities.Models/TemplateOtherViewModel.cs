using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class TemplateOtherViewModel : AuditableEntity
{
	public Guid? ProductId { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public string TemplateDescription { get; set; }
}
