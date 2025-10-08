using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class PlanDetailService : IPlanDetailService
{
	private readonly IUnitOfWork _uow;
	private readonly IMapper _mapper;

	public PlanDetailService(IUnitOfWork uow, IMapper mapper)
	{
		_uow = uow;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(Guid productid, Guid requestid)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			IEnumerable<MeasurementPlanViewModel> meass = await _uow.GetRepository<Measurement>().FindByAsync<MeasurementPlanViewModel>((Measurement x) => x.ProductId.Equals(productid), "Sort, Created");
			foreach (MeasurementPlanViewModel meas in meass)
			{
				RequestResult requestResult = await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(requestid) && ((object)x.MeasurementId).Equals((object?)meas.Id) && x.Sample.Equals(1), "");
				meas.IsSelect = requestResult != null;
			}
			res.Data = meass;
			res.Count = meass.Count();
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
			responseDto.Count = await _uow.GetRepository<PlanDetail>().CountAsync(args.Predicate, args.PredicateParameters);
			if (res.Count.Equals(0))
			{
				responseDto = res;
				responseDto.Data = await _uow.GetRepository<Measurement>().FindByAsync<MeasurementViewModel>((Measurement x) => x.ProductId.Equals(new Guid((string)args.PredicateParameters[0])), "Sort, Created");
				responseDto = res;
				responseDto.Count = await _uow.GetRepository<Measurement>().CountAsync((Measurement x) => x.ProductId.Equals(new Guid((string)args.PredicateParameters[0])));
			}
			else
			{
				responseDto = res;
				responseDto.Data = await _uow.GetRepository<PlanDetail>().FindByAsync<MeasurementViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Delete(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty))
			{
				throw new Exception("Id is null");
			}
			PlanDetail att = await _uow.GetRepository<PlanDetail>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find plan detail with id: {id}");
			}
			_uow.GetRepository<PlanDetail>().Delete(att);
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

	public async Task<ResponseDto> Save(PlanDetailViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new PlanDetail();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			PlanDetail att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<PlanDetail>().GetSingleAsync((PlanDetail x) => ((object)x.PlanId).Equals((object?)model.PlanId) && ((object)x.MeasurementId).Equals((object?)model.MeasurementId), "") != null)
				{
					throw new Exception("This plan detail already exist");
				}
				att = new PlanDetail();
				_mapper.Map(model, att);
				_uow.GetRepository<PlanDetail>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<PlanDetail>().GetSingleAsync((PlanDetail x) => ((object)x.PlanId).Equals((object?)model.PlanId) && ((object)x.MeasurementId).Equals((object?)model.MeasurementId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("This plan detail already exist");
				}
				att = await _uow.GetRepository<PlanDetail>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find plan detail with id: {model.Id}");
				}
				_mapper.Map(model, att);
				_uow.GetRepository<PlanDetail>().Update(att);
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
}
