using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class SimilarViewModel : AuditableEntity
{
	public string Value { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }
}
