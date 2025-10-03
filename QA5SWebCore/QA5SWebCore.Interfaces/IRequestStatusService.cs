using System;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;

namespace QA5SWebCore.Interfaces;

public interface IRequestStatusService
{
	Task<ResponseDto> Gets(Guid id);

	Task<ResponseDto> Gets(QueryArgs args);
}
