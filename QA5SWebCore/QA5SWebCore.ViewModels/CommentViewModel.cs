using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class CommentViewModel : AuditableEntity
{
	public string Content { get; set; }

	public string Link { get; set; }

	public Guid? RequestId { get; set; }
}
