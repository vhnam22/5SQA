using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Interfaces;

public interface IConstantService
{
	Task<ResponseDto> Gets(QueryArgs args);
}
