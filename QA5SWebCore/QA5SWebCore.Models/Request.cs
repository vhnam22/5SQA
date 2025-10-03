using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Request : AuditableEntity
{
	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public DateTimeOffset Date { get; set; }

	public Guid ProductId { get; set; }

	public virtual Product Product { get; set; }

	public int Sample { get; set; }

	public string CompletedBy { get; set; }

	public DateTimeOffset? Completed { get; set; }

	public string CheckedBy { get; set; }

	public DateTimeOffset? Checked { get; set; }

	public string ApprovedBy { get; set; }

	public DateTimeOffset? Approved { get; set; }

	public string Judgement { get; set; }

	[Required]
	public string Status { get; set; }

	[Required]
	public string Type { get; set; }

	public string Link { get; set; }

	public virtual ICollection<Comment> Comments { get; set; }

	public int? Quantity { get; set; }

	public string Lot { get; set; }

	public string Line { get; set; }

	public string Intention { get; set; }

	public DateTimeOffset? InputDate { get; set; }

	public string Supplier { get; set; }
}
