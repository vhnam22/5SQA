using System.Collections.Generic;

namespace _5S_QA_DL.APIContext;

public class ResponseDto
{
	public object Data { get; set; }

	public bool Success { get; set; }

	public ICollection<ResponseMessage> Messages { get; set; }

	public int Count { get; set; }
}
