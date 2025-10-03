using System.Collections.Generic;
using System.Net.Mail;

namespace QA5SWebCore.Utilities.Dtos;

public class EmailDto
{
	public string Subject { get; set; }

	public string Content { get; set; }

	public string ToEmail { get; set; }

	public string CcEmail { get; set; }

	public List<Attachment> Attachments { get; set; }
}
