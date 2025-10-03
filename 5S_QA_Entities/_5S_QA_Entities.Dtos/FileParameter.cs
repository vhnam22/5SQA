using System.IO;

namespace _5S_QA_Entities.Dtos;

public class FileParameter
{
	public Stream Data { get; private set; }

	public string FileName { get; private set; }

	public string ContentType { get; private set; }

	public FileParameter(Stream data)
		: this(data, null)
	{
	}

	public FileParameter(Stream data, string fileName)
		: this(data, fileName, null)
	{
	}

	public FileParameter(Stream data, string fileName, string contentType)
	{
		Data = data;
		FileName = fileName;
		ContentType = contentType;
	}
}
