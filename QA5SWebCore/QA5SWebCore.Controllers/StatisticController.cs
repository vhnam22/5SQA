using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Controllers;

[Route("api/[Controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class StatisticController : ControllerBase
{
	private readonly IStatisticService _statis;

	public StatisticController(IStatisticService statis)
	{
		_statis = statis;
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetProductNGForDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetProductNGForDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForOneDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForOneDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetDetailNGForOneDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetDetailNGForOneDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForOneDates([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForOneDates(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetProductNGForWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetProductNGForWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForOneWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForOneWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetDetailNGForOneWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetDetailNGForOneWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForOneWeeklys([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForOneWeeklys(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetProductNGForMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetProductNGForMonths(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForMonths(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForMonths(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetResultNGForOneMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetResultNGForOneMonths(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetDetailNGForOneMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetDetailNGForOneMonths(dto);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ResponseDto> GetTypeNGForOneMonths([FromBody] StatisticDto dto)
	{
		return await _statis.GetTypeNGForOneMonths(dto);
	}
}
