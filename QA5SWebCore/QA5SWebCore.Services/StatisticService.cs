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

public class StatisticService : IStatisticService
{
	private readonly IUnitOfWork _uow;

	public StatisticService(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<ResponseDto> GetProductNGForDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			for (int num = 7; num > 0; num--)
			{
				statistic.Values.Add(dto.Date.AddDays(-num + 1).ToString("ddd MM/dd"), 0.0);
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dto.Date.AddDays(-6.0).ToString("yyyy/MM/dd"),
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<DateTimeOffset?, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestDate;
			foreach (IGrouping<DateTimeOffset?, RequestStatisticViewModel> item in enumerable)
			{
				int num2 = 0;
				int num3 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num2 += item2.SampleNG.Value;
					num3 += (item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num2 / (double)num3 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				string key = item.Key.Value.DateTime.Date.ToString("ddd MM/dd");
				statistic.Values[key] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetResultNGForDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dto.Date.AddDays(-6.0).ToString("yyyy/MM/dd"),
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					statisticViewModel.Values.Add(dto.Date.AddDays(-num + 1).ToString("ddd MM/dd"), 0.0);
				}
				IEnumerable<IGrouping<DateTimeOffset?, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate;
				foreach (IGrouping<DateTimeOffset?, RequestStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					int num3 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num2 += item3.ResultNG.Value;
						num3 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num2 / (double)num3 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					statisticViewModel.Values[item2.Key.Value.ToString("ddd MM/dd")] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num4 = 7; num4 > 0; num4--)
				{
					string key = dto.Date.AddDays(-num4 + 1).ToString("ddd MM/dd");
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key]);
					statisticViewModel2.Values.Add(key, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dto.Date.AddDays(-6.0).ToString("yyyy/MM/dd"),
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestType;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					statisticViewModel.Values.Add(dto.Date.AddDays(-num + 1).ToString("ddd MM/dd"), 0.0);
				}
				IEnumerable<IGrouping<DateTimeOffset?, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate;
				foreach (IGrouping<DateTimeOffset?, RequestStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					int num3 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num2 += item3.ResultNG.Value;
						num3 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num2 / (double)num3 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					string key = item2.Key.Value.DateTime.Date.ToString("ddd MM/dd");
					statisticViewModel.Values[key] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num4 = 7; num4 > 0; num4--)
				{
					string key2 = dto.Date.AddDays(-num4 + 1).ToString("ddd MM/dd");
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key2]);
					statisticViewModel2.Values.Add(key2, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetResultNGForOneDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date=@0 && Request.Status!=@1 && Request.Status!=@2 && Request.Status!=@3";
			object[] predicateParameters = new string[4]
			{
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			for (int num = 0; num < enumerable.Count(); num++)
			{
				statistic.Values.Add(enumerable.ToList()[num].Key, 0.0);
			}
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				int num2 = 0;
				int num3 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num2 += item2.ResultNG.Value;
					num3 += (item2.MeasAll * item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num2 / (double)num3 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				statistic.Values[item.Key] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetDetailNGForOneDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date=@0 && Request.Status!=@1 && Request.Status!=@2 && Request.Status!=@3";
			object[] predicateParameters = new string[4]
			{
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Measurement.Sort";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.MeasurementName;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForOneDates(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date=@0 && Request.Status!=@1 && Request.Status!=@2 && Request.Status!=@3";
			object[] predicateParameters = new string[4]
			{
				dto.Date.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestType;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetProductNGForWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime startOfWeek = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			for (int num = 7; num > 0; num--)
			{
				DateTime dateTime2 = startOfWeek.AddDays((-num + 1) * 7);
				DateTime dateTime3 = startOfWeek.AddDays((-num + 1) * 7 + 6);
				string key = $"{dateTime2:MM/dd}~{dateTime3:MM/dd} Week {7 - num + 1}";
				statistic.Values.Add(key, 0.0);
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				startOfWeek.AddDays(-42.0).ToString("yyyy/MM/dd"),
				dateTime.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestDate.Value.Date.AddDays(0 - ((x.RequestDate.Value.Date.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (x.RequestDate.Value.Date.DayOfWeek - 1)));
			foreach (IGrouping<DateTime, RequestStatisticViewModel> item in enumerable)
			{
				DateTime key2 = item.Key;
				DateTime dateTime4 = key2.AddDays(6.0);
				int num2 = (int)(startOfWeek - key2).TotalDays / 7;
				string key3 = $"{key2:MM/dd}~{dateTime4:MM/dd} Week {7 - num2}";
				int num3 = 0;
				int num4 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num3 += item2.SampleNG.Value;
					num4 += (item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num3 / (double)num4 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				statistic.Values[key3] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetResultNGForWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime startOfWeek = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				startOfWeek.AddDays(-42.0).ToString("yyyy/MM/dd"),
				dateTime.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					DateTime dateTime2 = startOfWeek.AddDays((-num + 1) * 7);
					DateTime dateTime3 = startOfWeek.AddDays((-num + 1) * 7 + 6);
					string key = $"{dateTime2:MM/dd}~{dateTime3:MM/dd} Week {7 - num + 1}";
					statisticViewModel.Values.Add(key, 0.0);
				}
				IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate.Value.Date.AddDays(0 - ((x.RequestDate.Value.Date.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (x.RequestDate.Value.Date.DayOfWeek - 1)));
				foreach (IGrouping<DateTime, RequestStatisticViewModel> item2 in enumerable2)
				{
					DateTime key2 = item2.Key;
					DateTime dateTime4 = key2.AddDays(6.0);
					int num2 = (int)(startOfWeek - key2).TotalDays / 7;
					string key3 = $"{key2:MM/dd}~{dateTime4:MM/dd} Week {7 - num2}";
					int num3 = 0;
					int num4 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num3 += item3.ResultNG.Value;
						num4 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num3 / (double)num4 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					statisticViewModel.Values[key3] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num5 = 7; num5 > 0; num5--)
				{
					DateTime dateTime5 = startOfWeek.AddDays((-num5 + 1) * 7);
					DateTime dateTime6 = startOfWeek.AddDays((-num5 + 1) * 7 + 6);
					string key4 = $"{dateTime5:MM/dd}~{dateTime6:MM/dd} Week {7 - num5 + 1}";
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key4]);
					statisticViewModel2.Values.Add(key4, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime startOfWeek = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				startOfWeek.AddDays(-42.0).ToString("yyyy/MM/dd"),
				dateTime.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestType;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					DateTime dateTime2 = startOfWeek.AddDays((-num + 1) * 7);
					DateTime dateTime3 = startOfWeek.AddDays((-num + 1) * 7 + 6);
					string key = $"{dateTime2:MM/dd}~{dateTime3:MM/dd} Week {7 - num + 1}";
					statisticViewModel.Values.Add(key, 0.0);
				}
				IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate.Value.Date.AddDays(0 - ((x.RequestDate.Value.Date.DayOfWeek == DayOfWeek.Sunday) ? DayOfWeek.Saturday : (x.RequestDate.Value.Date.DayOfWeek - 1)));
				foreach (IGrouping<DateTime, RequestStatisticViewModel> item2 in enumerable2)
				{
					DateTime key2 = item2.Key;
					DateTime dateTime4 = key2.AddDays(6.0);
					int num2 = (int)(startOfWeek - key2).TotalDays / 7;
					string key3 = $"{key2:MM/dd}~{dateTime4:MM/dd} Week {7 - num2}";
					int num3 = 0;
					int num4 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num3 += item3.ResultNG.Value;
						num4 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num3 / (double)num4 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					statisticViewModel.Values[key3] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num5 = 7; num5 > 0; num5--)
				{
					DateTime dateTime5 = startOfWeek.AddDays((-num5 + 1) * 7);
					DateTime dateTime6 = startOfWeek.AddDays((-num5 + 1) * 7 + 6);
					string key4 = $"{dateTime5:MM/dd}~{dateTime6:MM/dd} Week {7 - num5 + 1}";
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key4]);
					statisticViewModel2.Values.Add(key4, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetResultNGForOneWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime dateTime = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime2 = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			for (int num = 0; num < enumerable.Count(); num++)
			{
				statistic.Values.Add(enumerable.ToList()[num].Key, 0.0);
			}
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				int num2 = 0;
				int num3 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num2 += item2.ResultNG.Value;
					num3 += (item2.MeasAll * item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num2 / (double)num3 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				statistic.Values[item.Key] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetDetailNGForOneWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime dateTime = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime2 = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Measurement.Sort";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.MeasurementName;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForOneWeeklys(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			int dayOfWeek = (int)dto.Date.DayOfWeek;
			DateTime dateTime = dto.Date.AddDays(-((dayOfWeek == 0) ? 6 : (dayOfWeek - 1)));
			DateTime dateTime2 = dto.Date.AddDays((dayOfWeek != 0) ? (7 - dayOfWeek) : 0);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestType;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetProductNGForMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int day = dto.Date.Day;
			DateTime dateTime = dto.Date.AddDays(-day + 1);
			DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			for (int num = 7; num > 0; num--)
			{
				string key = dateTime.AddMonths(-num + 1).ToString("yyyy/MM");
				statistic.Values.Add(key, 0.0);
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.AddMonths(-6).ToString("yyyy/MM/01"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestDate.Value.Date.AddDays(-x.RequestDate.Value.Date.Day + 1);
			foreach (IGrouping<DateTime, RequestStatisticViewModel> item in enumerable)
			{
				int num2 = 0;
				int num3 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num2 += item2.SampleNG.Value;
					num3 += (item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num2 / (double)num3 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				string key2 = item.Key.ToString("yyyy/MM");
				statistic.Values[key2] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetResultNGForMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int day = dto.Date.Day;
			DateTime startOfMonth = dto.Date.AddDays(-day + 1);
			DateTime dateTime = startOfMonth.AddMonths(1).AddDays(-1.0);
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				startOfMonth.AddMonths(-6).ToString("yyyy/MM/01"),
				dateTime.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					string key = startOfMonth.AddMonths(-num + 1).ToString("yyyy/MM");
					statisticViewModel.Values.Add(key, 0.0);
				}
				IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate.Value.Date.AddDays(-x.RequestDate.Value.Date.Day + 1);
				foreach (IGrouping<DateTime, RequestStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					int num3 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num2 += item3.ResultNG.Value;
						num3 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num2 / (double)num3 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					string key2 = item2.Key.ToString("yyyy/MM");
					statisticViewModel.Values[key2] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num4 = 7; num4 > 0; num4--)
				{
					string key3 = startOfMonth.AddMonths(-num4 + 1).ToString("yyyy/MM");
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key3]);
					statisticViewModel2.Values.Add(key3, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int day = dto.Date.Day;
			DateTime startOfMonth = dto.Date.AddDays(-day + 1);
			DateTime dateTime = startOfMonth.AddMonths(1).AddDays(-1.0);
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				startOfMonth.AddMonths(-6).ToString("yyyy/MM/01"),
				dateTime.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Date, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.RequestType;
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				for (int num = 7; num > 0; num--)
				{
					string key = startOfMonth.AddMonths(-num + 1).ToString("yyyy/MM");
					statisticViewModel.Values.Add(key, 0.0);
				}
				IEnumerable<IGrouping<DateTime, RequestStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestDate.Value.Date.AddDays(-x.RequestDate.Value.Date.Day + 1);
				foreach (IGrouping<DateTime, RequestStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					int num3 = 0;
					foreach (RequestStatisticViewModel item3 in item2)
					{
						num2 += item3.ResultNG.Value;
						num3 += (item3.MeasAll * item3.RequestSample * item3.ProductCavity).Value;
					}
					double value = (double)num2 / (double)num3 * 100.0;
					value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
					string key2 = item2.Key.ToString("yyyy/MM");
					statisticViewModel.Values[key2] = value;
				}
				statistics.Add(statisticViewModel);
			}
			if (statistics.Count > 0)
			{
				StatisticViewModel statisticViewModel2 = new StatisticViewModel
				{
					Name = "Average",
					Values = new Dictionary<string, double>()
				};
				for (int num4 = 7; num4 > 0; num4--)
				{
					string key3 = startOfMonth.AddMonths(-num4 + 1).ToString("yyyy/MM");
					double value2 = statistics.Average((StatisticViewModel x) => x.Values[key3]);
					statisticViewModel2.Values.Add(key3, value2);
				}
				statistics.Add(statisticViewModel2);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetResultNGForOneMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			int day = dto.Date.Day;
			DateTime dateTime = dto.Date.AddDays(-day + 1);
			DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
			StatisticViewModel statistic = new StatisticViewModel
			{
				Name = dto.Date.ToString("yyyy/MM/dd"),
				Values = new Dictionary<string, double>()
			};
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestStatistic>().FindByAsync<RequestStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			for (int num = 0; num < enumerable.Count(); num++)
			{
				statistic.Values.Add(enumerable.ToList()[num].Key, 0.0);
			}
			foreach (IGrouping<string, RequestStatisticViewModel> item in enumerable)
			{
				int num2 = 0;
				int num3 = 0;
				foreach (RequestStatisticViewModel item2 in item)
				{
					num2 += item2.ResultNG.Value;
					num3 += (item2.MeasAll * item2.RequestSample * item2.ProductCavity).Value;
				}
				double value = (double)num2 / (double)num3 * 100.0;
				value = Math.Round(value, 4, MidpointRounding.AwayFromZero);
				statistic.Values[item.Key] = value;
			}
			res.Data = statistic;
			res.Count = statistic.Values.Count;
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

	public async Task<ResponseDto> GetDetailNGForOneMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			int day = dto.Date.Day;
			DateTime dateTime = dto.Date.AddDays(-day + 1);
			DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Measurement.Sort";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.MeasurementName;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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

	public async Task<ResponseDto> GetTypeNGForOneMonths(StatisticDto dto)
	{
		ResponseDto res = new ResponseDto();
		try
		{
			List<StatisticViewModel> statistics = new List<StatisticViewModel>();
			int day = dto.Date.Day;
			DateTime dateTime = dto.Date.AddDays(-day + 1);
			DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Request.Date>=@0 && Request.Date<=@1 && Request.Status!=@2 && Request.Status!=@3 && Request.Status!=@4";
			object[] predicateParameters = new string[5]
			{
				dateTime.ToString("yyyy/MM/dd"),
				dateTime2.ToString("yyyy/MM/dd"),
				"Unactivated",
				"Activated",
				"Rejected"
			};
			queryArgs.PredicateParameters = predicateParameters;
			queryArgs.Order = "Request.Product.Code, Request.Type";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable = from x in await _uow.GetRepository<RequestResultStatistic>().FindByAsync<RequestResultStatisticViewModel>(queryArgs2.Order, queryArgs2.Page, queryArgs2.Limit, queryArgs2.Predicate, queryArgs2.PredicateParameters)
				group x by x.ProductCode;
			foreach (IGrouping<string, RequestResultStatisticViewModel> item in enumerable)
			{
				StatisticViewModel statisticViewModel = new StatisticViewModel
				{
					Name = item.Key,
					Values = new Dictionary<string, double>()
				};
				IEnumerable<IGrouping<string, RequestResultStatisticViewModel>> enumerable2 = from x in item
					group x by x.RequestType;
				for (int num = 0; num < enumerable2.Count(); num++)
				{
					statisticViewModel.Values.Add(enumerable2.ToList()[num].Key, 0.0);
				}
				foreach (IGrouping<string, RequestResultStatisticViewModel> item2 in enumerable2)
				{
					int num2 = 0;
					foreach (RequestResultStatisticViewModel item3 in item2)
					{
						num2 += item3.NG.Value;
					}
					statisticViewModel.Values[item2.Key] = num2;
				}
				statistics.Add(statisticViewModel);
			}
			res.Data = statistics;
			res.Count = statistics.Count;
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
