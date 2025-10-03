namespace _5S_QA_Entities.Dtos;

public class ChangePasswordDto
{
	public string Username { get; set; }

	public string OldPassword { get; set; }

	public string NewPassword { get; set; }

	public string ConfirmNewPassword { get; set; }
}
