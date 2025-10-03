using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Services;

public class RequestResultService : IRequestResultService
{
	private readonly IUnitOfWork _uow;

	public RequestResultService(IUnitOfWork unitOfWork)
	{
		_uow = unitOfWork;
	}

	public async Task<ResponseDto> Gets(Guid id, IEnumerable<string> lstmachinetype)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			RequestResult att1;
			RequestResult requestResult;
			if (lstmachinetype == null || lstmachinetype.Count().Equals(0))
			{
				att1 = await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(id) && !string.IsNullOrEmpty(x.Result), "Measurement.Sort DESC, Measurement.Created DESC");
				requestResult = await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(id), "Sample DESC");
			}
			else
			{
				att1 = await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(id) && !string.IsNullOrEmpty(x.Result) && lstmachinetype.Any((string i) => i.Equals(x.Measurement.MachineType.Name)), "Measurement.Sort DESC, Measurement.Created DESC");
				requestResult = await _uow.GetRepository<RequestResult>().GetSingleAsync((RequestResult x) => x.RequestId.Equals(id) && lstmachinetype.Any((string i) => i.Equals(x.Measurement.MachineType.Name)), "Sample DESC");
			}
			res.Data = att1 ?? new RequestResult();
			res.Count = requestResult?.Sample ?? 1;
		}
		catch
		{
			res.Data = new RequestResult();
			res.Count = 1;
		}
		return res;
	}

	public async Task<ResponseDto> Gets(Guid id, int sample, IEnumerable<string> lstmachinetype)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			Request request = await _uow.GetRepository<Request>().FindByIdAsync(id);
			if (request == null)
			{
				return new ResponseDto(Enumerable.Empty<ResultFullViewModel>(), 0);
			}
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
			List<ResultFullViewModel> results = new List<ResultFullViewModel>();
			IEnumerable<ResultFullViewModel> enumerable = await _uow.GetRepository<Measurement>().FindByAsync<ResultFullViewModel>((Measurement x) => x.ProductId.Equals(request.ProductId), "Sort, Created");
			if (lstmachinetype != null && lstmachinetype.Count() > 0)
			{
				enumerable = enumerable.Where((ResultFullViewModel x) => lstmachinetype.Any((string y) => y.Equals(x.MachineTypeName)));
			}
			ResultFullViewModel item;
			int i;
			foreach (ResultFullViewModel item3 in enumerable)
			{
				item = item3;
				i = 1;
				while (true)
				{
					if (i > product.Cavity)
					{
						break;
					}
					ResultFullViewModel resultFullViewModel = await _uow.GetRepository<RequestResult>().GetSingleAsync<ResultFullViewModel>((RequestResult x) => x.RequestId.Equals(id) && ((object)x.MeasurementId).Equals((object?)item.MeasurementId) && x.Sample.Equals(sample) && x.Cavity.Equals(i));
					if (resultFullViewModel != null)
					{
						results.Add(resultFullViewModel);
					}
					else
					{
						ResultFullViewModel item2 = new ResultFullViewModel
						{
							RequestId = id,
							MeasurementId = item.MeasurementId,
							MeasurementCode = item.MeasurementCode,
							MeasurementName = item.MeasurementName,
							MeasurementUnit = item.MeasurementUnit,
							ImportantName = item.ImportantName,
							Name = item.Name,
							Value = item.Value,
							Unit = item.Unit,
							Upper = item.Upper,
							Lower = item.Lower,
							LSL = item.LSL,
							USL = item.USL,
							LCL = item.LCL,
							UCL = item.UCL,
							MachineTypeName = item.MachineTypeName,
							Formula = item.Formula,
							Id = Guid.Empty,
							Sample = sample,
							Cavity = i,
							Coordinate = item.Coordinate,
							Created = item.Created,
							CreatedBy = item.CreatedBy,
							Modified = item.Modified,
							ModifiedBy = item.ModifiedBy,
							IsActivated = item.IsActivated
						};
						results.Add(item2);
					}
					i++;
				}
			}
			res.Data = results;
			res.Count = results.Count;
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

	public async Task<ResponseDto> Gets(Guid id, Guid idmeas)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			if (id.Equals(Guid.Empty) || idmeas.Equals(Guid.Empty))
			{
				return new ResponseDto(Enumerable.Empty<ResultFullViewModel>(), 0);
			}
			Request request = await _uow.GetRepository<Request>().FindByIdAsync(id);
			if (request == null)
			{
				return new ResponseDto(Enumerable.Empty<ResultFullViewModel>(), 0);
			}
			Product product = await _uow.GetRepository<Product>().FindByIdAsync(request.ProductId);
			List<ResultFullViewModel> results = new List<ResultFullViewModel>();
			ResultFullViewModel meas = await _uow.GetRepository<Measurement>().FindByIdAsync<ResultFullViewModel>(idmeas);
			meas.RequestId = request.Id;
			meas.RequestName = request.Name;
			meas.Id = Guid.Empty;
			IEnumerable<ResultFullViewModel> source = await _uow.GetRepository<RequestResult>().FindByAsync<ResultFullViewModel>((RequestResult x) => x.RequestId.Equals(id) && ((object)x.MeasurementId).Equals((object?)idmeas));
			for (int i = 1; i <= request.Sample; i++)
			{
				int j;
				for (j = 1; j <= product.Cavity; j++)
				{
					ResultFullViewModel resultFullViewModel = source.Where((ResultFullViewModel x) => x.Sample.Equals(i) && x.Cavity.Equals(j)).FirstOrDefault();
					if (resultFullViewModel == null)
					{
						ResultFullViewModel item = new ResultFullViewModel
						{
							RequestId = meas.RequestId,
							MeasurementId = meas.MeasurementId,
							MeasurementCode = meas.MeasurementCode,
							MeasurementName = meas.MeasurementName,
							MeasurementUnit = meas.MeasurementUnit,
							ImportantName = meas.ImportantName,
							Name = meas.Name,
							Value = meas.Value,
							Unit = meas.Unit,
							Upper = meas.Upper,
							Lower = meas.Lower,
							LSL = meas.LSL,
							USL = meas.USL,
							LCL = meas.LCL,
							UCL = meas.UCL,
							MachineTypeName = meas.MachineTypeName,
							Id = Guid.Empty,
							Sample = i,
							Cavity = j,
							Created = meas.Created,
							CreatedBy = meas.CreatedBy,
							Modified = meas.Modified,
							ModifiedBy = meas.ModifiedBy,
							IsActivated = meas.IsActivated
						};
						results.Add(item);
					}
					else
					{
						results.Add(resultFullViewModel);
					}
				}
			}
			res.Data = results;
			res.Count = results.Count;
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
			responseDto.Data = await _uow.GetRepository<RequestResult>().FindByAsync<ResultFullViewModel>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
			responseDto = res;
			responseDto.Count = await _uow.GetRepository<RequestResult>().CountAsync(args.Predicate, args.PredicateParameters);
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

	public async Task<ResponseDto> Save(List<FormulaViewModel> lstmodel, RequestResultViewModel model, int digit)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			foreach (FormulaViewModel item in lstmodel)
			{
				if (!item.IsChange && !item.IsChangeOther)
				{
					continue;
				}
				string judge = null;
				if (!string.IsNullOrEmpty(item.Result))
				{
					if (!string.IsNullOrEmpty(item.Unit))
					{
						double.TryParse(item.Value, out var result);
						double num = Math.Round(result + item.Upper.Value, digit, MidpointRounding.AwayFromZero);
						double num2 = Math.Round(result + item.Lower.Value, digit, MidpointRounding.AwayFromZero);
						double.TryParse(item.Result, out var result2);
						if (num2 > result2)
						{
							judge = "NG-";
						}
						else if (result2 > num)
						{
							judge = "NG+";
						}
						else
						{
							judge = "OK";
							if (item.WarnUpper.HasValue && result2 > Math.Round(result + item.WarnUpper.Value, digit, MidpointRounding.AwayFromZero))
							{
								judge = "OK+";
							}
							if (item.WarnLower.HasValue && result2 < Math.Round(result + item.WarnLower.Value, digit, MidpointRounding.AwayFromZero))
							{
								judge = "OK-";
							}
						}
					}
					else
					{
						string text = item.Result.Trim().ToUpper();
						judge = ((!text.Equals("NG") && !text.Equals("STOP") && !text.Equals("NOGO")) ? "OK" : "NG");
					}
				}
				List<History> list = new List<History>();
				if (!string.IsNullOrEmpty(item.History))
				{
					list = JsonConvert.DeserializeObject<List<History>>(item.History);
				}
				if (item.IsChange && !string.IsNullOrEmpty(item.History))
				{
					History history = new History();
					history.Created = item.Modified;
					history.CreatedBy = item.StaffName;
					history.MachineName = item.MachineNameOld;
					history.Value = item.ResultOld;
					list.Add(history);
				}
				string history2 = JsonConvert.SerializeObject(list);
				RequestResult resultRequest = new RequestResult
				{
					Id = item.Id,
					RequestId = model.RequestId.Value,
					MeasurementId = item.MeasurementId,
					Name = item.Name,
					Value = item.Value,
					Upper = item.Upper,
					Lower = item.Lower,
					Unit = item.Unit,
					Result = item.Result,
					ResultOrigin = item.ResultOrigin,
					Judge = judge,
					MachineName = ((item.IsChangeByFormula && item.MeasurementId != model.MeasurementId) ? "Formuler" : item.MachineName),
					StaffName = item.StaffName,
					History = history2,
					Modified = DateTimeOffset.Now,
					Sample = item.Sample.Value,
					Cavity = item.Cavity.Value,
					IsActivated = true
				};
				if (resultRequest.Id.Equals(Guid.Empty))
				{
					_uow.GetRepository<RequestResult>().Add(resultRequest);
				}
				else
				{
					_uow.GetRepository<RequestResult>().Update(resultRequest);
				}
				await _uow.CommitAsync();
				if (resultRequest.MeasurementId.Equals(model.MeasurementId))
				{
					res.Data = resultRequest;
				}
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

	public async Task<ResponseDto> GetSampleHasResults(Guid idrequest, Guid idplan)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			IEnumerable<RequestResult> results = await _uow.GetRepository<RequestResult>().FindByAsync((RequestResult x) => x.RequestId.Equals(idrequest) && !string.IsNullOrEmpty(x.Result), "");
			if (!idplan.Equals(Guid.Empty))
			{
				IEnumerable<PlanDetail> plandetails = await _uow.GetRepository<PlanDetail>().FindByAsync((PlanDetail x) => x.PlanId.Equals(idplan), "");
				results = results.Where((RequestResult x) => plandetails.Any((PlanDetail c) => c.MeasurementId.Equals(x.MeasurementId)));
			}
			IEnumerable<IGrouping<int, RequestResult>> source = from x in results
				group x by x.Sample;
			res.Count = source.Count();
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

	public async Task<ResponseDto> GetsForChart(ResultForChartDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<ResultFullViewModel> results = new List<ResultFullViewModel>();
			IEnumerable<Request> reqs = await _uow.GetRepository<Request>().FindByAsync("Date DESC, Created DESC", 1, dto.Limit, "ProductId=@0 && Date<=@1", new object[2] { dto.ProductId, dto.Date });
			int i = reqs.Count() - 1;
			while (true)
			{
				if (i < 0)
				{
					break;
				}
				results.AddRange(await _uow.GetRepository<RequestResult>().FindByAsync<ResultFullViewModel>((RequestResult x) => x.RequestId == reqs.ElementAt(i).Id && !string.IsNullOrEmpty(x.Result)));
				i--;
			}
			res.Data = results;
			res.Count = results.Count();
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

	public async Task<ResponseDto> GetForStatistics(StatisticDetailDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Judge.Contains(@0) && Request.Status!=@1 && Request.Status!=@2 && Request.Status!=@3";
			object[] predicateParameters = new string[4] { "NG", "Unactivated", "Activated", "Rejected" };
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Created, Request.Name, Measurement.Sort, Measurement.Created, Sample, Cavity";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			switch (dto.Type)
			{
			case "Day":
				queryArgs2.Predicate += " && Request.Date=@4";
				predicateParameters = new string[5]
				{
					"NG",
					"Unactivated",
					"Activated",
					"Rejected",
					dto.Date.ToString("yyyy/MM/dd")
				};
				queryArgs2.PredicateParameters = predicateParameters;
				break;
			case "Week":
			{
				int day = (int)dto.Date.DayOfWeek;
				DateTime dateTime3 = dto.Date.AddDays(-((day == 0) ? 6 : (day - 1)));
				DateTime dateTime4 = dto.Date.AddDays((day != 0) ? (7 - day) : 0);
				queryArgs2.Predicate += " && Request.Date>=@4 && Request.Date<=@5 ";
				predicateParameters = new string[6]
				{
					"NG",
					"Unactivated",
					"Activated",
					"Rejected",
					dateTime3.ToString("yyyy/MM/dd"),
					dateTime4.ToString("yyyy/MM/dd")
				};
				queryArgs2.PredicateParameters = predicateParameters;
				break;
			}
			case "Month":
			{
				int day = dto.Date.Day;
				DateTime dateTime = dto.Date.AddDays(-day + 1);
				DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
				queryArgs2.Predicate += " && Request.Date>=@4 && Request.Date<=@5";
				predicateParameters = new string[6]
				{
					"NG",
					"Unactivated",
					"Activated",
					"Rejected",
					dateTime.ToString("yyyy/MM/dd"),
					dateTime2.ToString("yyyy/MM/dd")
				};
				queryArgs2.PredicateParameters = predicateParameters;
				break;
			}
			}
			if (dto.MeasurementId == Guid.Empty)
			{
				queryArgs2.Predicate += $" && Request.Type=\"{dto.MeasurementName}\" && Request.ProductId=\"{dto.ProductId}\"";
			}
			else
			{
				queryArgs2.Predicate += $" && MeasurementId=\"{dto.MeasurementId}\"";
			}
			IEnumerable<StatisticResultViewModel> source = (IEnumerable<StatisticResultViewModel>)(res.Data = await _uow.GetRepository<RequestResult>().FindByAsync<StatisticResultViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters));
			res.Count = source.Count();
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

	private int countDigitDouble(string value)
	{
		try
		{
			return value.Split('.')[1].Length;
		}
		catch
		{
			return 0;
		}
	}
}
