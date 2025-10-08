using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
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

public class AQLService : IAQLService
{
	private readonly IUnitOfWork _uow;

	private readonly IConfiguration _configuration;

	private readonly IMapper _mapper;

	public AQLService(IUnitOfWork uow, IConfiguration configuration, IMapper mapper)
	{
		_uow = uow;
		_configuration = configuration;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<AQL>().FindByAsync<AQLViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<AQL>().CountAsync(args.Predicate, args.PredicateParameters);
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
			AQL att = (await _uow.GetRepository<AQL>().FindByIdAsync(id)) ?? throw new Exception($"Can't find AQL with id: {id}");
			_uow.GetRepository<AQL>().Delete(att);
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

	public async Task<ResponseDto> Save(AQLViewModel model)
	{
		ResponseDto res = new ResponseDto();
		new AQL();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			string cryptedString = _configuration["Key"];
			ConfigApp configApp = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
			if (!configApp.Funtion.Contains("QC2208"))
			{
				throw new Exception("Unlicensed");
			}
			AQL att;
			if (model.Id.Equals(Guid.Empty))
			{
				if (await _uow.GetRepository<AQL>().GetSingleAsync((AQL x) => x.ProductId == model.ProductId && x.Type.Equals(model.Type) && ((object)x.InputQuantity).Equals((object?)model.InputQuantity), "") != null)
				{
					throw new Exception("Master data sampling already exist");
				}
				att = new AQL();
				_mapper.Map(model, att);
				AQL aQL = await _uow.GetRepository<AQL>().GetSingleAsync((AQL x) => ((object)x.ProductId).Equals((object?)model.ProductId), "Sort DESC, Created DESC");
				if (aQL != null)
				{
					att.Sort = aQL.Sort + 1;
				}
				_uow.GetRepository<AQL>().Add(att);
			}
			else
			{
				if (await _uow.GetRepository<AQL>().GetSingleAsync((AQL x) => x.ProductId == model.ProductId && x.Type.Equals(model.Type) && ((object)x.InputQuantity).Equals((object?)model.InputQuantity) && !x.Id.Equals(model.Id), "") != null)
				{
					throw new Exception("Master data sampling no already exist");
				}
				att = await _uow.GetRepository<AQL>().FindByIdAsync(model.Id);
				if (att == null)
				{
					throw new Exception($"Can't find AQL with id: {model.Id}");
				}
				model.Sort = att.Sort;
				_mapper.Map(model, att);
				_uow.GetRepository<AQL>().Update(att);
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
			if (await _uow.GetRepository<AQL>().FindByIdAsync(idfrom) == null)
			{
				throw new Exception($"Can't find AQL with idform: {idfrom}");
			}
			AQL aqlfrom = await _uow.GetRepository<AQL>().FindByIdAsync(idfrom);
			AQL aQL = await _uow.GetRepository<AQL>().FindByIdAsync(idto);
			if (aqlfrom == null || aQL == null)
			{
				throw new Exception($"Can't find AQL with idform: {idfrom} or idto: {idto}");
			}
			_ = string.Empty;
			string allAsync = ((aqlfrom.Sort <= aQL.Sort) ? $"Update AQLs Set Sort = Sort - 1 Where Sort > {aqlfrom.Sort} And Sort <= {aQL.Sort} And ProductId = '{aqlfrom.ProductId}'" : $"Update AQLs Set sort = Sort + 1 Where Sort < {aqlfrom.Sort} And Sort >= {aQL.Sort} And ProductId = '{aqlfrom.ProductId}'");
			_uow.GetRepository<AQL>().SetAllAsync(allAsync);
			aqlfrom.Sort = aQL.Sort;
			_uow.GetRepository<AQL>().Update(aqlfrom);
			await _uow.CommitAsync();
			res.Data = _mapper.Map<AQLViewModel>(aqlfrom);
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

	public async Task<ResponseDto> SaveList(IEnumerable<AQLViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null)
			{
				throw new Exception("Models is null");
			}
			Product pro = (await _uow.GetRepository<Product>().FindByIdAsync(models.First().ProductId.Value)) ?? throw new Exception($"Can't find product with id: {models.First().ProductId}");
			int sort = -1;
			AQL aQL = await _uow.GetRepository<AQL>().GetSingleAsync((AQL x) => x.ProductId.Equals(pro.Id), "Sort DESC, Created DESC");
			if (aQL != null)
			{
				sort = aQL.Sort;
			}
			List<AQL> anons = new List<AQL>();
			foreach (var item in models.Select((AQLViewModel Value, int Index) => new { Value, Index }))
			{
				AQLViewModel model = item.Value;
				int num = item.Index + 1;
				string Message = string.Empty;
				if (string.IsNullOrEmpty(model.Type))
				{
					Message += ((Message == string.Empty) ? $"No {num}: Type is empty;" : " Type is empty;");
				}
				if (model.InputQuantity.HasValue && !int.TryParse(model.InputQuantity.ToString(), out var result))
				{
					Message += ((Message == string.Empty) ? $"No {num}: Input quantity incorrect format;" : " Input quantity incorrect format;");
				}
				if (!model.Sample.HasValue)
				{
					Message += ((Message == string.Empty) ? $"No {num}: Sample is empty;" : " Sample is empty;");
				}
				else if (!int.TryParse(model.Sample.ToString(), out result))
				{
					Message += ((Message == string.Empty) ? $"No {num}: Sample incorrect format;" : " Sample incorrect format;");
				}
				model.Sort = sort + num;
				aQL = await _uow.GetRepository<AQL>().GetSingleAsync((AQL x) => x.ProductId == pro.Id && x.Type == model.Type && x.InputQuantity == model.InputQuantity, "Sort DESC, Created DESC");
				if (aQL != null)
				{
					model.Id = aQL.Id;
				}
				anons.Add(_mapper.Map<AQL>(model));
				if (!string.IsNullOrEmpty(Message))
				{
					res.Messages.Add(new ResponseMessage
					{
						Code = "Error",
						Message = Message
					});
				}
			}
			if (res.Messages.Count > 0)
			{
				return res;
			}
			foreach (AQL item2 in anons)
			{
				await Save(_mapper.Map<AQLViewModel>(item2));
			}
			res.Data = anons;
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

	public async Task<ResponseDto> Samples(AQLDto model)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (model == null)
			{
				throw new Exception("Model is null");
			}
			List<LimitViewModel> samples = new List<LimitViewModel>();
			IEnumerable<IGrouping<string, AQL>> enumerable = from x in await _uow.GetRepository<AQL>().FindByAsync((AQL x) => x.ProductId == model.ProductId, "Type ASC, InputQuantity DESC")
				group x by x.Type;
			foreach (IGrouping<string, AQL> item in enumerable)
			{
				if (!item.First().InputQuantity.HasValue)
				{
					samples.Add(new LimitViewModel
					{
						Type = item.Key,
						Sample = item.First().Sample
					});
					continue;
				}
				int num = 0;
				foreach (AQL item2 in item)
				{
					if (item2.InputQuantity >= model.InputQuantity)
					{
						num = item2.Sample;
					}
				}
				if (num > 0)
				{
					samples.Add(new LimitViewModel
					{
						Type = item.Key,
						Sample = num
					});
				}
			}
			IEnumerable<Measurement> meass = await _uow.GetRepository<Measurement>().FindByAsync((Measurement x) => x.ProductId == model.ProductId, "Sort, Created");
			IEnumerable<ResultRank> source = await _uow.GetRepository<ResultRank>().FindByAsync((ResultRank x) => x.RequestId == model.RequestId, "");
			List<LimitViewModel> list = new List<LimitViewModel>();
			foreach (Measurement meas in meass)
			{
				string type = Regex.Match(meas.Name, "(?<=\\().*(?=\\))").Value;
				LimitViewModel limitViewModel = samples.FirstOrDefault((LimitViewModel x) => x.Type == type);
				ResultRank resultRank = source.FirstOrDefault((ResultRank x) => x.MeasurementId == meas.Id);
				int? num2 = ((limitViewModel == null) ? new int?(0) : limitViewModel.Sample);
				if (resultRank != null)
				{
					num2 += resultRank.Sample;
				}
				if (limitViewModel != null)
				{
					list.Add(new LimitViewModel
					{
						MeasurementId = meas.Id,
						Type = type,
						Sample = num2
					});
				}
			}
			res.Data = list;
			res.Count = list.Count;
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
