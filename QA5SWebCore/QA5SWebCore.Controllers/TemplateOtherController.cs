using System;
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
public class TemplateOtherController : ControllerBase
{
	private readonly ITemplateOtherService _other;

	public TemplateOtherController(ITemplateOtherService other)
	{
		_other = other;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _other.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _other.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] TemplateOtherViewModel model)
	{
		return await _other.Save(model);
	}
}
