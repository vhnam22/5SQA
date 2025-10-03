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

public class MetadataValueService : IMetadataValueService
{
	private readonly IUnitOfWork _uow;

	public MetadataValueService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<MetadataValue>().FindByAsync<MetadataValueViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<MetadataValue>().CountAsync(args.Predicate, args.PredicateParameters);
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
			MetadataValue att = await _uow.GetRepository<MetadataValue>().FindByIdAsync(id);
			if (att == null)
			{
				throw new Exception($"Can't find metadatavalue with id: {id}");
			}
			_uow.GetRepository<MetadataValue>().Delete(att);
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

	public async Task<ResponseDto> Save(MetadataValueViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new MetadataValue();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			MetadataValue att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.TypeId).Equals((object?)model.TypeId), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = new MetadataValue();
				Mapper.Map(model, att);
				_uow.GetRepository<MetadataValue>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => (x.Code.Equals(model.Code) || x.Name.Equals(model.Name)) && ((object)x.TypeId).Equals((object?)model.TypeId) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Code or name already exist");
				}
				att = await _uow.GetRepository<MetadataValue>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find metadatavalue with id: {model.Id}");
				}
				Mapper.Map(model, att);
				_uow.GetRepository<MetadataValue>().Update(att);
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

	public async Task<ResponseDto> SaveRanks(IEnumerable<MetadataValueViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<MetadataValueViewModel> anons = new List<MetadataValueViewModel>();
			foreach (MetadataValueViewModel model in models)
			{
				MetadataValue metadataValue = await _uow.GetRepository<MetadataValue>().GetSingleAsync((MetadataValue x) => x.Code == model.Code && ((object)x.TypeId).Equals((object?)model.TypeId), "");
				if (metadataValue != null)
				{
					model.Id = metadataValue.Id;
				}
				if (metadataValue != null && string.IsNullOrEmpty(model.Value))
				{
					await Delete(model.Id);
				}
				else if (!string.IsNullOrEmpty(model.Value))
				{
					await Save(model);
				}
				anons.Add(model);
			}
			res.Data = anons;
			res.Count = anons.Count;
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

	public async Task<ResponseDto> GetDecimals(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<string> decimals = new List<string>();
			foreach (MetadataValue item in await _uow.GetRepository<MetadataValue>().FindByAsync(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters))
			{
				if (!string.IsNullOrEmpty(item.Value))
				{
					decimals.Add(item.Name + "#" + item.Value);
				}
			}
			res.Data = decimals;
			res.Count = decimals.Count();
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
