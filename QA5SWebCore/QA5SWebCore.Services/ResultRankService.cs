using System;
using System.Collections.Generic;
using System.Linq;
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

public class ResultRankService : IResultRankService
{
	private readonly IUnitOfWork _uow;

	private readonly WebSocketMessageHandler _socket;

	private readonly IMapper _mapper;

	public ResultRankService(IUnitOfWork uow, WebSocketMessageHandler socket, IMapper mapper)
	{
		_uow = uow;
		_socket = socket;
		_mapper = mapper;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<ResultRank>().FindByAsync<ResultRankViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<ResultRank>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> SaveList(IEnumerable<ResultRankViewModel> models)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (models == null || models.Count() == 0)
			{
				throw new Exception("Models is null");
			}
			foreach (ResultRankViewModel model in models)
			{
				ResultRank item = new ResultRank();
				_mapper.Map(model, item);
				ResultRank resultRank = await _uow.GetRepository<ResultRank>().GetSingleAsync((ResultRank x) => x.RequestId == model.RequestId && x.MeasurementId == model.MeasurementId, "");
				if (resultRank == null)
				{
					_uow.GetRepository<ResultRank>().Add(item);
					continue;
				}
				item.Id = resultRank.Id;
				_uow.GetRepository<ResultRank>().Update(item);
			}
			RequestViewModel att = (await _uow.GetRepository<Request>().FindByIdAsync<RequestViewModel>(models.First().RequestId.Value)) ?? throw new Exception($"Can't find request with id: {models.First().RequestId}");
			int? num = models.Max((ResultRankViewModel x) => x.Total);
			if (att.Sample < num)
			{
				att.Sample = num.Value;
			}
			Request request = new Request();
			_mapper.Map(att, request);
			_uow.GetRepository<Request>().Update(request);
			await _uow.CommitAsync();
			res.Data = models;
			res.Count = models.Count();
			string message = JsonConvert.SerializeObject(att);
			await _socket.SendMessageToAll(message);
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
