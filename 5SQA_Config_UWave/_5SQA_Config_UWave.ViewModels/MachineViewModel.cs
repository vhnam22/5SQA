using _5SQA_Config_UWave.Abstracts;

namespace _5SQA_Config_UWave.ViewModels;

public class MachineViewModel : AuditableEntity
{
	public string ChanelId { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string Model { get; set; }

	public string Serial { get; set; }

	public string FactoryName { get; set; }

	public string MachineTypeName { get; set; }

	public string Status { get; set; }
}
