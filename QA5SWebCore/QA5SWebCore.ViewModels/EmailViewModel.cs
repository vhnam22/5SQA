using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class EmailViewModel : AuditableEntity
{
	public string Name { get; set; }

	public string Password { get; set; }

	public string Header { get; set; }

	public string Footer { get; set; }
}
