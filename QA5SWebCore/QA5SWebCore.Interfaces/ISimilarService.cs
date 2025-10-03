using System;
using System.Threading.Tasks;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.ViewModels;

namespace QA5SWebCore.Interfaces;

public interface ISimilarService
{
	Task<ResponseDto> Gets(Guid id);

	Task<ResponseDto> Save(SimilarViewModel model);

	Task<ResponseDto> Delete(SimilarViewModel model);
}
