using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface ISettingService
{
	Task<ResponseDto> Gets(QueryArgs args);

	Task<ResponseDto> Save(SettingViewModel model);
}
