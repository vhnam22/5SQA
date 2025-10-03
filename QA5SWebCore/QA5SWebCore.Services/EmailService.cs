using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class EmailService : IEmailService
{
	private readonly IUnitOfWork _uow;

	private EmailSettingDto _emailSetting
	{
		get
		{
			Email single = _uow.GetRepository<Email>().GetSingle((Email x) => x.Id != null, "");
			return new EmailSettingDto
			{
				Server = "smtp.gmail.com",
				Port = 587,
				UseDefaultCredentials = false,
				EnableSSL = true,
				SenderEmail = ((single == null) ? "" : single.Name),
				Username = ((single == null) ? "" : single.Name),
				Password = ((single == null) ? "" : single.Password)
			};
		}
	}

	public EmailService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<bool> SendEmailAsync(EmailDto email)
	{
		return await Task.Run(() => SendEmail(email));
	}

	public bool SendEmail(EmailDto email)
	{
		try
		{
			if (email != null && !string.IsNullOrEmpty(email.ToEmail))
			{
				SmtpClient sMTPClient = GetSMTPClient();
				if (sMTPClient != null)
				{
					return Send(email, sMTPClient);
				}
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private SmtpClient GetSMTPClient()
	{
		if (_emailSetting != null)
		{
			return new SmtpClient
			{
				UseDefaultCredentials = _emailSetting.UseDefaultCredentials,
				EnableSsl = _emailSetting.EnableSSL,
				Host = _emailSetting.Server,
				Port = _emailSetting.Port,
				Timeout = 300000,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password)
			};
		}
		return null;
	}

	private bool Send(EmailDto email, SmtpClient client)
	{
		try
		{
			MailMessage mailMessage = new MailMessage
			{
				From = new MailAddress(_emailSetting.SenderEmail),
				Subject = email.Subject,
				Body = email.Content,
				IsBodyHtml = true,
				BodyEncoding = Encoding.UTF8,
				SubjectEncoding = Encoding.UTF8
			};
			mailMessage.To.Clear();
			mailMessage.CC.Clear();
			mailMessage.Attachments.Clear();
			string[] array = email.ToEmail.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string addresses in array)
			{
				mailMessage.To.Add(addresses);
			}
			if (email.CcEmail != null)
			{
				string[] array2 = email.CcEmail.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string addresses2 in array2)
				{
					mailMessage.CC.Add(addresses2);
				}
			}
			if (email.Attachments != null)
			{
				foreach (Attachment attachment in email.Attachments)
				{
					mailMessage.Attachments.Add(attachment);
				}
			}
			client.Send(mailMessage);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Email>().FindByAsync<EmailViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Email>().CountAsync(args.Predicate, args.PredicateParameters);
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}

	public async Task<ResponseDto> Save(EmailViewModel model)
	{
		ResponseDto res = new ResponseDto();
		Email att = new Email();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			if (model.Id.Equals(Guid.Empty))
			{
				Mapper.Map(model, att);
				_uow.GetRepository<Email>().Add(att);
			}
			else
			{
				att = await _uow.GetRepository<Email>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find email with id: {model.Id}");
				}
				Mapper.Map(model, att);
				_uow.GetRepository<Email>().Update(att);
			}
			await _uow.CommitAsync();
			res.Data = att;
		}
		catch (Exception ex)
		{
			res.Messages.Add(new ResponseMessage
			{
				Code = "Exception",
				Message = ex.Message
			});
		}
		return res;
	}
}
