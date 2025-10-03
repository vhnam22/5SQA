using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RequestStatusController : ControllerBase
{
	private readonly IRequestStatusService _status;

	private readonly IUnitOfWork _uow;

	public RequestStatusController(IRequestStatusService status, IUnitOfWork uow)
	{
		_status = status;
		_uow = uow;
	}

	[HttpGet("{id}")]
	public async Task<ResponseDto> Gets(Guid id)
	{
		return await _status.Gets(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _status.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveListWithId([FromBody] IEnumerable<Guid> ids)
	{
		ResponseDto res = new ResponseDto();
		List<object> anons = new List<object>();
		foreach (Guid id in ids)
		{
			RequestStatus model = await _uow.GetRepository<RequestStatus>().FindByIdAsync(id);
			model.Status = "ACCEPTABLE";
			_uow.GetRepository<RequestStatus>().Update(model);
			await _uow.CommitAsync();
			anons.Add(model);
		}
		res.Data = anons;
		return res;
	}
}
