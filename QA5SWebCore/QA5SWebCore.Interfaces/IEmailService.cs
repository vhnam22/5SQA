using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IEmailService
{
	Task<bool> SendEmailAsync(EmailDto email);

	bool SendEmail(EmailDto email);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Save(EmailViewModel model);
}
