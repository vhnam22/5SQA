using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class EmailViewModel : AuditableEntity
{
	public string Name { get; set; }

	public string Password { get; set; }

	public string Header { get; set; }

	public string Footer { get; set; }
}
