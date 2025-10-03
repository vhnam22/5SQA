using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.Models;

public class Setting : AuditableEntity
{
	public string Group { get; set; }

	public string Name { get; set; }

	public bool IsChecked { get; set; }
}
