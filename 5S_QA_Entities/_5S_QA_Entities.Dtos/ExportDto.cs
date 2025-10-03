using System;

namespace _5S_QA_Entities.Dtos;

public class ExportDto
{
	public Guid Id { get; set; }

	public Guid? TemplateId { get; set; }

	public string Name { get; set; }

	public string Type { get; set; }

	public string Page { get; set; }
}
