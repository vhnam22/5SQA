using _5S_QA_Entities.Abstracts;

namespace _5S_QA_Entities.Models;

public class SettingViewModel : AuditableEntity
{
	public string Group { get; set; }

	public string Name { get; set; }

	public bool? IsChecked { get; set; }
}
