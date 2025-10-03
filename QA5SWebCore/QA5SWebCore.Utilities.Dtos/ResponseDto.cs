using System.Collections.Generic;
using System.Linq;

namespace QA5SWebCore.Utilities.Dtos;

public class ResponseDto
{
	public object Data { get; set; }

	public bool Success => !Messages.Any();

	public List<ResponseMessage> Messages { get; set; }

	public int Count { get; set; }

	public ResponseDto()
	{
		Messages = new List<ResponseMessage>();
	}

	public ResponseDto(object data, int count)
		: this()
	{
		Data = data;
		Count = count;
	}
}
