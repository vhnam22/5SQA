using System;
using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class ResultFileViewModel : AuditableEntity
{
	public Guid? RequestId { get; set; }

	public string RequestName { get; set; }

	public string Link { get; set; }
}
