using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Email : AuditableEntity
{
	public string Name { get; set; }

	public string Password { get; set; }

	public string Header { get; set; }

	public string Footer { get; set; }
}
