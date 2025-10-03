using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class AQLViewModel : AuditableEntity
{
	public string Type { get; set; }

	public int? InputQuantity { get; set; }

	public int? Sample { get; set; }

	public int? Sort { get; set; }

	public Guid? ProductId { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }
}
