using System.Collections.Generic;

namespace _5SQA_Config_UWave.Funtions;

public class ResponseDto
{
	public object Data { get; set; }

	public bool Success { get; set; }

	public ICollection<ResponseMessage> Messages { get; set; }

	public int Count { get; set; }
}
