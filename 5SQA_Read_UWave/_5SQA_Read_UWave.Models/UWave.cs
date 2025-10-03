namespace _5SQA_Read_UWave.Models;

public class UWave
{
	public string Header { get; set; }

	public string GroupId { get; set; }

	public string ChanelId { get; set; }

	public string Digit { get; set; }

	public string Data { get; set; }

	public string Unit { get; set; }

	public UWave(string data)
	{
		if (data.Length >= 19)
		{
			Header = data.Substring(0, 3);
			GroupId = data.Substring(3, 2);
			ChanelId = data.Substring(5, 2);
			Digit = data.Substring(7, 1);
			Data = data.Substring(8, 11);
			Unit = ((data.Substring(19) == "M") ? "mm" : "in");
		}
	}
}
