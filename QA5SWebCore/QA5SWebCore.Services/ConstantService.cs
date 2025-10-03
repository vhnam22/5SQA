using System;
using System.Threading.Tasks;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Services;

public class ConstantService : IConstantService
{
	private readonly IUnitOfWork _uow;

	public ConstantService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<Constant>().FindByAsync(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<Constant>().CountAsync(args.Predicate, args.PredicateParameters);
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
