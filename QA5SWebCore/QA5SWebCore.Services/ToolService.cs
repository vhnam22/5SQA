using System;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Sockets;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class ToolService : IToolService
{
	private readonly IUnitOfWork _uow;

	private readonly WebSocketMessageHandler _socketHandler;
	private readonly IMapper _mapper;

    public ToolService(IUnitOfWork unitOfWork, WebSocketMessageHandler socketHandler)
	{
		_uow = unitOfWork;
		_socketHandler = socketHandler;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Tool>().FindByAsync<ToolViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Tool>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Save(ToolViewModel model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null || model.MachineId.Equals(Guid.Empty) || !model.MachineId.HasValue)
			{
				throw new Exception("Model is null");
			}
			Tool att = await _uow.GetRepository<Tool>().GetSingleAsync((Tool x) => ((object)x.MachineId).Equals((object?)model.MachineId), "");
			if (att == null)
			{
				att = new Tool();
				Mapper.Map(model, att);
				_uow.GetRepository<Tool>().Add(att);
			}
			else
			{
				model.Id = att.Id;
				model.Created = att.Created;
				model.IsActivated = att.IsActivated;
				Mapper.Map(model, att);
				_uow.GetRepository<Tool>().Update(att);
			}
			await _uow.CommitAsync();
			res.Data = att;
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

	public async Task<ResponseDto> Result(ToolResultDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null || string.IsNullOrEmpty(model.Result) || (model.MachineId.Equals(Guid.Empty) && string.IsNullOrEmpty(model.MachineName)))
			{
				throw new Exception("Machine name or result tool is null");
			}
			if (double.Parse(model.Result) == 0.0)
			{
				ToolViewModel model2 = new ToolViewModel
				{
					MachineId = model.MachineId
				};
				res = await Save(model2);
				ToolResultViewModel value = new ToolResultViewModel
				{
					MachineName = model.MachineName,
					Result = model.Result,
					Unit = model.Unit
				};
				string message = JsonConvert.SerializeObject(value);
				await _socketHandler.SendMessageToAll(message);
			}
			else
			{
				ToolResultViewModel att = await _uow.GetRepository<Tool>().GetSingleAsync<ToolResultViewModel>((Tool x) => ((object)x.MachineId).Equals((object?)model.MachineId));
				if (att != null && att.TabletId.HasValue)
				{
					att.Result = model.Result;
					att.Unit = model.Unit;
					string message2 = JsonConvert.SerializeObject(att);
					await _socketHandler.SendMessage(att.TabletId.ToString(), message2);
				}
				else
				{
					MachineViewModel machineViewModel = (await _uow.GetRepository<Machine>().FindByIdAsync<MachineViewModel>(model.MachineId)) ?? throw new Exception("Machine is null");
					att = new ToolResultViewModel
					{
						MachineId = machineViewModel.Id,
						MachineName = machineViewModel.Name,
						MachineTypeName = machineViewModel.MachineTypeName,
						Result = model.Result,
						Unit = model.Unit
					};
					string message3 = JsonConvert.SerializeObject(att);
					await _socketHandler.SendMessageToAll(message3);
				}
				res.Data = att;
			}
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
