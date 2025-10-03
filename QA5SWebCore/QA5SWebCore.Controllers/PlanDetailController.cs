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
public class PlanDetailController : ControllerBase
{
	private readonly IPlanDetailService _detail;

	public PlanDetailController(IPlanDetailService detail)
	{
		_detail = detail;
	}

	[HttpGet("{productid}/{requestid}")]
	public async Task<ResponseDto> Gets(Guid productid, Guid requestid)
	{
		return await _detail.Gets(productid, requestid);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _detail.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _detail.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] PlanDetailViewModel model)
	{
		return await _detail.Save(model);
	}
}
