using System.Collections.Generic;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IResultRankService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> SaveList(IEnumerable<ResultRankViewModel> models);
}
