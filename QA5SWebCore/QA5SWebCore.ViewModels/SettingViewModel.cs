using QA5SWebCore.Data.Abstracts;

namespace QA5SWebCore.ViewModels;

public class SettingViewModel : AuditableEntity
{
	public string Group { get; set; }

	public string Name { get; set; }

	public bool? IsChecked { get; set; }
}
