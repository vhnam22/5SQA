using System.Collections.Generic;

namespace _5SQA_Config_UWave.ViewModels;

public class Config
{
	public string APIUrl { get; set; }

	public string ComName { get; set; }

	public string IME { get; set; }

	public List<Mapper> Mappers { get; set; }
}
