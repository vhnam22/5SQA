using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface IToolService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Save(ToolViewModel model);

	Task<ResponseDto> Result(ToolResultDto model);
}
