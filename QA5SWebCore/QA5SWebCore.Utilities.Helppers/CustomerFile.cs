namespace QA5SWebCore.Utilities.Helppers;

public class CustomerFile
{
	public byte[] FileContents { get; set; }

	public string FileName { get; set; }

	public CustomerFile()
	{
	}

	public CustomerFile(byte[] fileContents, string fileName)
	{
		FileContents = fileContents;
		FileName = fileName;
	}
}
