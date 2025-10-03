using System;
using System.ComponentModel.DataAnnotations;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Comment : AuditableEntity
{
	[Required]
	public string Content { get; set; }

	public string Link { get; set; }

	public Guid RequestId { get; set; }

	public virtual Request Request { get; set; }
}
