using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class EmailController : Controller
{
	private readonly IEmailService _service;

	public EmailController(IEmailService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _service.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] EmailViewModel model)
	{
		return await _service.Save(model);
	}
}
