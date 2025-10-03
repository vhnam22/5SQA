namespace QA5SWebCore.Utilities.Dtos;

public class ChangeMyPasswordDto
{
	public string Username { get; set; }

	public string OldPassword { get; set; }

	public string NewPassword { get; set; }

	public string ConfirmNewPassword { get; set; }
}
