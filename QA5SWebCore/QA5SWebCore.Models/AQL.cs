using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class AQL : AuditableEntity
{
	public string Type { get; set; }

	public int? InputQuantity { get; set; }

	public int Sample { get; set; }

	public int Sort { get; set; }

	public Guid ProductId { get; set; }

	public virtual Product Product { get; set; }
}
