namespace QA5SWebCore.Utilities.Dtos;

public class EmailSettingDto
{
	public int Port { get; set; }

	public string Server { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string SenderEmail { get; set; }

	public bool EnableSSL { get; set; }

	public bool UseDefaultCredentials { get; set; }
}
