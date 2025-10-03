using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class PlanService : IPlanService
{
	private readonly IUnitOfWork _uow;

	public PlanService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Plan>().FindByAsync<PlanViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Plan>().CountAsync(args.Predicate, args.PredicateParameters);
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
			Plan att = await _uow.GetRepository<Plan>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find plan with id: {id}");
			}
			IEnumerable<PlanDetail> entities = await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(id), "");
			_uow.GetRepository<PlanDetail>().Delete(entities);
			_uow.GetRepository<Plan>().Delete(att);
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

	public async Task<ResponseDto> Save(PlanDto model)
	{
		ResponseDto res = new ResponseDto();
		Plan att = new Plan();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			if (model.Id.Equals(Guid.Empty))
			{
				att = await _uow.GetRepository<Plan>().GetSingleAsync((Plan x) => ((object)x.ProductId).Equals((object?)model.ProductId) && ((object)x.StageId).Equals((object?)model.StageId), "");
				if (att != null)
				{
					throw new Exception("This plan already exist");
				}
				att = new Plan();
				Mapper.Map(model, att);
				Plan plan = await _uow.GetRepository<Plan>().GetSingleAsync((Plan x) => ((object)x.ProductId).Equals((object?)model.ProductId), "Sort DESC, Created DESC");
				if (plan != null)
				{
					att.Sort = plan.Sort + 1;
				}
				_uow.GetRepository<Plan>().Add(att);
				List<PlanDetail> list = new List<PlanDetail>();
				foreach (Guid measurementId in model.MeasurementIds)
				{
					list.Add(new PlanDetail
					{
						PlanId = att.Id,
						MeasurementId = measurementId
					});
				}
				_uow.GetRepository<PlanDetail>().Add(list);
			}
			else
			{
				att = await _uow.GetRepository<Plan>().GetSingleAsync((Plan x) => ((object)x.ProductId).Equals((object?)model.ProductId) && ((object)x.StageId).Equals((object?)model.StageId) && !x.Id.Equals(model.Id), "");
				if (att != null)
				{
					throw new Exception("This plan already exist");
				}
				att = await _uow.GetRepository<Plan>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find plan with id: {model.Id}");
				}
				int sort = att.Sort;
				Mapper.Map(model, att);
				att.Sort = sort;
				_uow.GetRepository<Plan>().Update(att);
				IEnumerable<PlanDetail> entities = await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(att.Id), "");
				_uow.GetRepository<PlanDetail>().Delete(entities);
				List<PlanDetail> list2 = new List<PlanDetail>();
				foreach (Guid measurementId2 in model.MeasurementIds)
				{
					list2.Add(new PlanDetail
					{
						PlanId = att.Id,
						MeasurementId = measurementId2
					});
				}
				_uow.GetRepository<PlanDetail>().Add(list2);
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

	public async Task<ResponseDto> Move(Guid idfrom, Guid idto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (await _uow.GetRepository<Plan>().FindByIdAsync(idfrom) == null)
			{
				throw new Exception($"Can't find plan with idform: {idfrom}");
			}
			Plan measfrom = await _uow.GetRepository<Plan>().FindByIdAsync(idfrom);
			Plan plan = await _uow.GetRepository<Plan>().FindByIdAsync(idto);
			if (measfrom == null || plan == null)
			{
				throw new Exception($"Can't find plan with idform: {idfrom} or idto: {idto}");
			}
			_ = string.Empty;
			string allAsync = ((measfrom.Sort <= plan.Sort) ? $"Update Plans Set Sort = Sort - 1 Where Sort > {measfrom.Sort} And Sort <= {plan.Sort} And ProductId = '{measfrom.ProductId}'" : $"Update Plans Set sort = Sort + 1 Where Sort < {measfrom.Sort} And Sort >= {plan.Sort} And ProductId = '{measfrom.ProductId}'");
			_uow.GetRepository<Plan>().SetAllAsync(allAsync);
			measfrom.Sort = plan.Sort;
			_uow.GetRepository<Plan>().Update(measfrom);
			await _uow.CommitAsync();
			res.Data = Mapper.Map<PlanViewModel>(measfrom);
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
