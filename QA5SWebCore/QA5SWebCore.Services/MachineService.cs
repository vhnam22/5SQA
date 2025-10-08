using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class MachineService : IMachineService
{
	private readonly IUnitOfWork _uow;

	private readonly IConfiguration _configuration;

	private readonly IMapper _mapper;

	public MachineService(IUnitOfWork uow, IConfiguration configuration, IMapper mapper)
	{
		_uow = uow;
		_configuration = configuration;
		_mapper = mapper;
	}

	public async Task<ResponseDto> GetForTools(Guid idtablet, QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			IEnumerable<MachineForToolViewModel> machines = await _uow.GetRepository<Machine>().FindByAsync<MachineForToolViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			IEnumerable<Tool> source = await _uow.GetRepository<Tool>().FindByAsync((Tool x) => ((object)x.TabletId).Equals((object?)idtablet), "");
			foreach (MachineForToolViewModel machine in machines)
			{
				if (source.Where((Tool x) => x.MachineId.Equals(machine.Id)).Count().Equals(0))
				{
					machine.IsHasTool = false;
				}
				else
				{
					machine.IsHasTool = true;
				}
			}
			res.Data = machines;
			ResponseDto responseDto = res;
			responseDto.Count = await _uow.GetRepository<Machine>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Machine>().FindByAsync<MachineViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Machine>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Delete(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			Machine att = await _uow.GetRepository<Machine>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find machine with id: {id}");
			}
			IEnumerable<Tool> entities = await _uow.GetRepository<Tool>().FindByAsync((Tool x) => ((object)x.MachineId).Equals((object?)id) || ((object)x.TabletId).Equals((object?)id), "");
			_uow.GetRepository<Tool>().Delete(entities);
			_uow.GetRepository<Machine>().Delete(att);
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

	public async Task<ResponseDto> Save(MachineViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new Machine();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			Machine att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<Machine>().GetSingleAsync((Machine x) => x.Code.Equals(model.Code) || x.Name.Equals(model.Name) || (x.Mark != (int?)null && ((object)x.Mark).Equals((object?)model.Mark)), "") != null)
				{
					throw new Exception("Code or name or cable code already exist");
				}
				att = new Machine();
				_mapper.Map(model, att);
				_uow.GetRepository<Machine>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<Machine>().GetSingleAsync((Machine x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name) || (x.Mark != (int?)null && ((object)x.Mark).Equals((object?)model.Mark))) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code or name or cable code already exist");
				}
				att = await _uow.GetRepository<Machine>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find machine with id: {model.Id}");
				}
				_mapper.Map(model, att);
				_uow.GetRepository<Machine>().Update(att);
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

	public async Task<ResponseDto> GetLiences(string ime)
	{
		List<MachineLienceViewModel> machines = new List<MachineLienceViewModel>();
		ResponseDto res = new ResponseDto
		{
			Data = machines
		};
		try
		{
			if (string.IsNullOrEmpty(ime))
			{
				throw new Exception("Ime is null");
			}
			string text = _configuration["Lience"];
			if (string.IsNullOrEmpty(text))
			{
				return res;
			}
			LienceDto lienceDto = JsonSerializer.Deserialize<List<LienceDto>>(DesLogHelper.Decrypt(text, "GetBytes")).FirstOrDefault((LienceDto x) => x.IME == ime);
			if (lienceDto == null)
			{
				return res;
			}
			foreach (string mark in lienceDto.Marks)
			{
				MachineLienceViewModel machineLienceViewModel = await _uow.GetRepository<Machine>().GetSingleAsync<MachineLienceViewModel>((Machine x) => x.Mark != (int?)null && x.Mark == (int?)int.Parse(mark));
				if (machineLienceViewModel != null)
				{
					machines.Add(machineLienceViewModel);
				}
			}
			res.Data = machines;
			res.Count = machines.Count;
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
