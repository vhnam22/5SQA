using System;
using System.Threading.Tasks;
using AutoMapper;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class SettingService : ISettingService
{
	private readonly IUnitOfWork _uow;
	private readonly IMapper _mapper;

	public SettingService(IUnitOfWork uow, IMapper mapper)
	{
		_uow = uow;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Setting>().FindByAsync<SettingViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Setting>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Save(SettingViewModel model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Setting att = await _uow.GetRepository<Setting>().GetSingleAsync((Setting x) => x.Name == model.Name, "");
			if (att == null)
			{
				att = new Setting();
				_mapper.Map(model, att);
				_uow.GetRepository<Setting>().Add(att);
			}
			else
			{
				model.Id = att.Id;
				_mapper.Map(model, att);
				_uow.GetRepository<Setting>().Update(att);
			}
			await _uow.CommitAsync();
			res.Data = att;
			res.Count = 1;
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
