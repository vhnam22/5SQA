using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IRequestResultService
{
	Task<ResponseDto> Gets(Guid id, IEnumerable<string> lstmachinetype);

	Task<ResponseDto> Gets(Guid id, int sample, IEnumerable<string> lstmachinetype);

	Task<ResponseDto> Gets(Guid id, Guid idmeas);

	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Save(List<FormulaViewModel> lstmodel, RequestResultViewModel model, int digit);

	Task<ResponseDto> GetSampleHasResults(Guid idrequest, Guid idplan);

	Task<ResponseDto> GetsForChart(ResultForChartDto dto);

	Task<ResponseDto> GetForStatistics(StatisticDetailDto dto);
}
