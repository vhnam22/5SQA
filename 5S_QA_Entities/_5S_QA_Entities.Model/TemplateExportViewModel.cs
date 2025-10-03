using System;

namespace _5S_QA_Entities.Model;

public class TemplateExportViewModel
{
	public Guid Id { get; set; }

	public DateTimeOffset Created { get; set; }

	public DateTimeOffset Modified { get; set; }

	public bool IsActivated { get; set; }

	public string Description { get; set; }

	public string FileDisplayName { get; set; }

	public string FileUniqueName { get; set; }

	public Guid? ProductId { get; set; }
}
