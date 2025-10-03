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
public class SettingController : Controller
{
	private readonly ISettingService _service;

	public SettingController(ISettingService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<ResponseDto> Gets([FromBody] QueryArgs args)
	{
		return await _service.Gets(args);
	}

	[HttpPost]
	public async Task<ResponseDto> Save([FromBody] SettingViewModel model)
	{
		return await _service.Save(model);
	}

	[HttpPost]
	public async Task<ResponseDto> SaveList([FromBody] IEnumerable<SettingViewModel> models)
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
		foreach (SettingViewModel model in models)
		{
			anons.Add((await Save(model)).Data);
		}
		res.Data = anons;
		return res;
	}
}
