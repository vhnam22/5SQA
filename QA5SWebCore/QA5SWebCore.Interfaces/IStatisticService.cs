using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Interfaces;

public interface IStatisticService
{
	Task<ResponseDto> GetProductNGForDates(StatisticDto dto);

	Task<ResponseDto> GetResultNGForDates(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForDates(StatisticDto dto);

	Task<ResponseDto> GetResultNGForOneDates(StatisticDto dto);

	Task<ResponseDto> GetDetailNGForOneDates(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForOneDates(StatisticDto dto);

	Task<ResponseDto> GetProductNGForWeeklys(StatisticDto dto);

	Task<ResponseDto> GetResultNGForWeeklys(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForWeeklys(StatisticDto dto);

	Task<ResponseDto> GetResultNGForOneWeeklys(StatisticDto dto);

	Task<ResponseDto> GetDetailNGForOneWeeklys(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForOneWeeklys(StatisticDto dto);

	Task<ResponseDto> GetProductNGForMonths(StatisticDto dto);

	Task<ResponseDto> GetResultNGForMonths(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForMonths(StatisticDto dto);

	Task<ResponseDto> GetResultNGForOneMonths(StatisticDto dto);

	Task<ResponseDto> GetDetailNGForOneMonths(StatisticDto dto);

	Task<ResponseDto> GetTypeNGForOneMonths(StatisticDto dto);
}
