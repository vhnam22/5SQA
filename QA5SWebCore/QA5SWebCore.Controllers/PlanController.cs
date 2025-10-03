using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class PlanController : ControllerBase
{
	private readonly IPlanService _plan;

	public PlanController(IPlanService plan)
	{
		_plan = plan;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _plan.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _plan.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] PlanDto model)
	{
		return await _plan.Save(model);
	}

	[HttpPost("{idfrom}/{idto}")]
	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		return await _plan.Move(idfrom, idto);
	}
}
