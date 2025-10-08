using System;
using System.Threading.Tasks;
using AutoMapper;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class TemplateOtherService : ITemplateOtherService
{
	private readonly IUnitOfWork _uow;
	private readonly IMapper _mapper;

	public TemplateOtherService(IUnitOfWork uow, IMapper mapper)
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
			responseDto.Data = await _uow.GetRepository<TemplateOther>().FindByAsync<TemplateOtherViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<TemplateOther>().CountAsync(args.Predicate, args.PredicateParameters);
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
			TemplateOther att = await _uow.GetRepository<TemplateOther>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find template other with id: {id}");
			}
			_uow.GetRepository<TemplateOther>().Delete(att);
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

	public async Task<ResponseDto> Save(TemplateOtherViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new TemplateOther();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			TemplateOther att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<TemplateOther>().GetSingleAsync((TemplateOther x) => x.ProductId == model.ProductId && x.TemplateId == model.TemplateId, "") != null)
				{
					throw new Exception("Template already exist");
				}
				att = new TemplateOther();
				_mapper.Map(model, att);
				_uow.GetRepository<TemplateOther>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<TemplateOther>().GetSingleAsync((TemplateOther x) => x.ProductId == model.ProductId && x.TemplateId == model.TemplateId && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Template already exist");
				}
				att = await _uow.GetRepository<TemplateOther>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find template other with id: {model.Id}");
				}
				_mapper.Map(model, att);
				_uow.GetRepository<TemplateOther>().Update(att);
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
