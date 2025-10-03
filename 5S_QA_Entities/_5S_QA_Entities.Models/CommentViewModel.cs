using System;
using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class CommentViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public string Content { get; set; }

	public string Link { get; set; }
}
