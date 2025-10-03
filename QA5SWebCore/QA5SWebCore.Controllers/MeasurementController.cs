using System;
using System.Collections.Generic;
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
public class MeasurementController : ControllerBase
{
	private readonly IMeasurementService _meas;

	public MeasurementController(IMeasurementService meas)
	{
		_meas = meas;
	}

	[HttpGet("{productid}/{planid}")]
	public async Task<ResponseDto> Gets(Guid productid, Guid planid)
	{
		return await _meas.Gets(productid, planid);
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _meas.Gets(args);
	}

	[HttpDelete("{id}")]
	public async Task<ResponseDto> Delete(Guid id)
	{
		return await _meas.Delete(id);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] MeasurementViewModel model)
	{
		return await _meas.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<MeasurementViewModel> models)
	{
		return await _meas.SaveList(models);
	}

	[HttpPost("{idfrom}/{idto}")]
	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		return await _meas.Move(idfrom, idto);
	}

	[HttpPost("{id}")]
	public async Task<ResponseDto> Gets(Guid id, [FromBody] IEnumerable<string> lstmachinetype)
	{
		return await _meas.Gets(id, lstmachinetype);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveCoordinate([FromBody] MeasurementDto model)
	{
		return await _meas.SaveCoordinate(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveCoordinates([FromBody] IEnumerable<MeasurementDto> models)
	{
		ResponseDto res = new ResponseDto();
		if (models == null)
		{
			res.Messages.Add(new ResponseMessage
			{
				Message = "Models is null"
			});
			return res;
		}
		List<object> anons = new List<object>();
		foreach (MeasurementDto model in models)
		{
			anons.Add((await SaveCoordinate(model)).Data);
		}
		res.Data = anons;
		return res;
	}
}
