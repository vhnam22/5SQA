using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class SimilarViewModel : AuditableEntity
{
	public string Value { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }
}
