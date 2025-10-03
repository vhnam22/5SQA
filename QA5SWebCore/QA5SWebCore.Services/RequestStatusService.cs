using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class RequestStatusService : IRequestStatusService
{
	private readonly IUnitOfWork _uow;

	public RequestStatusService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> Gets(Guid id)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			Request request = await _uow.GetRepository<Request>().FindByIdAsync(id);
			if (request == null)
			{
				throw new Exception($"Can't find request with id: {id}");
			}
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
			List<PlanViewModel> plans = (await _uow.GetRepository<Plan>().FindByAsync<PlanViewModel>((Plan x) => x.ProductId.Equals(request.ProductId), "Sort")).ToList();
			Dictionary<Guid, List<Guid?>> idmeass = new Dictionary<Guid, List<Guid?>>();
			if (plans.Count.Equals(0))
			{
				int value = await _uow.GetRepository<Measurement>().CountAsync((Measurement x) => x.ProductId.Equals(request.ProductId));
				plans.Add(new PlanViewModel
				{
					TotalDetails = value
				});
			}
			else
			{
				foreach (PlanViewModel plan in plans)
				{
					List<Guid?> ids = new List<Guid?>();
					foreach (PlanDetail item in await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(plan.Id), ""))
					{
						ids.Add(item.MeasurementId);
					}
					idmeass.Add(plan.Id, ids);
				}
			}
			IEnumerable<RequestResult> resultAll = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(request.Id) && !string.IsNullOrEmpty(x.Result), "");
			IEnumerable<RequestStatusViewModel> source = await _uow.GetRepository<RequestStatus>().FindByAsync<RequestStatusViewModel>((RequestStatus x) => x.RequestId.Equals(request.Id));
			List<RequestStatusViewModel> list = new List<RequestStatusViewModel>();
			for (int i = 1; i <= request.Sample; i++)
			{
				foreach (PlanViewModel plan2 in plans)
				{
					RequestStatusViewModel requestStatusViewModel = source.Where((RequestStatusViewModel x) => x.Sample.Equals(i) && ((!(plan2.Id == Guid.Empty)) ? x.PlanId.Equals(plan2.Id) : (!x.PlanId.HasValue))).FirstOrDefault();
					string status = ((requestStatusViewModel == null) ? string.Empty : requestStatusViewModel.Status);
					string completedBy = requestStatusViewModel?.CompletedBy;
					DateTimeOffset? completed = requestStatusViewModel?.Completed;
					string checkedBy = requestStatusViewModel?.CompletedBy;
					DateTimeOffset? dateTimeOffset = requestStatusViewModel?.Completed;
					string approvedBy = requestStatusViewModel?.ApprovedBy;
					DateTimeOffset? approved = requestStatusViewModel?.Approved;
					int num;
					int num2;
					if (plan2.Id == Guid.Empty)
					{
						num = resultAll.Where((RequestResult x) => x.Sample.Equals(i) && x.Judge.Contains("OK")).Count();
						num2 = resultAll.Where((RequestResult x) => x.Sample.Equals(i) && x.Judge.Contains("NG")).Count();
					}
					else
					{
						idmeass.TryGetValue(plan2.Id, out var checks);
						num = resultAll.Where((RequestResult x) => x.Sample.Equals(i) && x.Judge.Contains("OK") && checks.Any((Guid? y) => y.Equals(x.MeasurementId))).Count();
						num2 = resultAll.Where((RequestResult x) => x.Sample.Equals(i) && x.Judge.Contains("NG") && checks.Any((Guid? y) => y.Equals(x.MeasurementId))).Count();
					}
					int? empty = plan2.TotalDetails * product.Cavity - num - num2;
					string planName = ((requestStatusViewModel == null) ? plan2.StageName : requestStatusViewModel.PlanName);
					Guid? planId = ((requestStatusViewModel == null) ? new Guid?(plan2.Id) : requestStatusViewModel.PlanId);
					list.Add(new RequestStatusViewModel
					{
						Sample = i,
						PlanId = planId,
						PlanName = planName,
						OK = num,
						NG = num2,
						Empty = empty,
						Status = status,
						CompletedBy = completedBy,
						Completed = completed,
						CheckedBy = checkedBy,
						Checked = dateTimeOffset,
						ApprovedBy = approvedBy,
						Approved = approved
					});
				}
			}
			res.Data = list;
			res.Count = list.Count();
		}
		catch
		{
			res.Data = new List<RequestStatusViewModel>();
			res.Count = 1;
		}
		return res;
	}

	public async Task<ResponseDto> Gets(QueryArgs args)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			ResponseDto responseDto = res;
			responseDto.Data = await _uow.GetRepository<RequestStatus>().FindByAsync<RequestStatusViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<RequestStatus>().CountAsync(args.Predicate, args.PredicateParameters);
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
