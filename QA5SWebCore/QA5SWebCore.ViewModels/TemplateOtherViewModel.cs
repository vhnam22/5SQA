using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class TemplateOtherViewModel : AuditableEntity
{
	public Guid? ProductId { get; set; }

	public Guid? TemplateId { get; set; }

	public string TemplateName { get; set; }

	public string TemplateDescription { get; set; }
}
