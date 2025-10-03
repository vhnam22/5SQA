using System;
using System.Net;
using System.Net.Mail;

namespace QA5SWebCore.Utilities.Helppers;

public static class SendEmail
{
	public static string Send(string _subject, string _email, string _description)
	{
		string text = "thoithoithoi2015@gmail.com";
		string password = "spkt120695";
		string result = "Email Sent Successfully";
		try
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.To.Add(_email);
			mailMessage.From = new MailAddress(text);
			mailMessage.Subject = _subject;
			mailMessage.Body = _description;
			mailMessage.IsBodyHtml = true;
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Host = "smtp.gmail.com";
			smtpClient.Credentials = new NetworkCredential(text, password);
			smtpClient.Port = 587;
			smtpClient.EnableSsl = true;
			smtpClient.Send(mailMessage);
			return result;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}
}
