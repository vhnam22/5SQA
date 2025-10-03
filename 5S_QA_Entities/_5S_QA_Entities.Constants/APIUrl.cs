using System.Configuration;

namespace _5S_QA_Entities.Constants;

public static class APIUrl
{
	public static string APIHost = ConfigurationManager.ConnectionStrings["APIHost"].ConnectionString;

	public const string APILogin = "/api/AuthUser/Login";

	public const string APILogout = "/api/AuthUser/Logout/{id}";

	public const string APIUserReset = "/api/AuthUser/ResetPassword";

	public const string APIUserChangePass = "/api/AuthUser/ChangeMyPassword";

	public const string APIUserUpdateImage = "/api/AuthUser/UpdateImage/{id}";

	public const string APIUser = "/api/AuthUser/Gets";

	public const string APIUserDelete = "/api/AuthUser/Delete/{id}";

	public const string APIUserSave = "/api/AuthUser/Save";

	public const string APIUserFuntions = "/api/AuthUser/GetFuntions";

	public const string APIMetadataValue = "/api/MetadataValue/Gets";

	public const string APIMetadataValueDelete = "/api/MetadataValue/Delete/{id}";

	public const string APIMetadataValueSave = "/api/MetadataValue/Save";

	public const string APIMetadataValueSaveRank = "/api/MetadataValue/SaveRanks";

	public const string APIMetadataValueDecimals = "/api/MetadataValue/GetDecimals";

	public const string APIMachine = "/api/Machine/Gets";

	public const string APIMachineDelete = "/api/Machine/Delete/{id}";

	public const string APIMachineSave = "/api/Machine/Save";

	public const string APIProduct = "/api/Product/Gets";

	public const string APIProductDelete = "/api/Product/Delete/{id}";

	public const string APIProductSave = "/api/Product/Save";

	public const string APIProductSaveList = "/api/Product/SaveList";

	public const string APIProductUpdateImage = "/api/Product/UpdateImage/{id}";

	public const string APIProductCopy = "/api/Product/Copy/{id}";

	public const string APIProductMove = "/api/Product/Move/{idfrom}/{idto}";

	public const string APIMeasurementForPlan = "/api/Measurement/Gets/{productid}/{id}";

	public const string APIMeasurement = "/api/Measurement/Gets";

	public const string APIMeasurementDelete = "/api/Measurement/Delete/{id}";

	public const string APIMeasurementSave = "/api/Measurement/Save";

	public const string APIMeasurementSaveList = "/api/Measurement/SaveList";

	public const string APIMeasurementMove = "/api/Measurement/Move/{idfrom}/{idto}";

	public const string APIMeasurementSaveCoordinate = "/api/Measurement/SaveCoordinate";

	public const string APIMeasurementSaveCoordinates = "/api/Measurement/SaveCoordinates";

	public const string APIResultWithSample = "/api/RequestResult/Gets/{id}/{sample}";

	public const string APIResultSampleComplete = "/api/RequestResult/Gets/{id}";

	public const string APIResult = "/api/RequestResult/Gets";

	public const string APIResultSave = "/api/RequestResult/Save";

	public const string APIResultSaveList = "/api/RequestResult/SaveList";

	public const string APIResultCountSampleHasResult = "/api/RequestResult/GetSampleHasResults/{productid}/{id}";

	public const string APIResultForStatistic = "/api/RequestResult/GetForStatistics";

	public const string APIRequestJudgement = "/api/Request/Gets/{id}";

	public const string APIRequest = "/api/Request/Gets";

	public const string APIRequestDelete = "/api/Request/Delete/{id}";

	public const string APIRequestSave = "/api/Request/Save";

	public const string APIRequestSaveList = "/api/Request/SaveList";

	public const string APIRequestActive = "/api/Request/Active";

	public const string APIRequestActiveList = "/api/Request/ActiveList";

	public const string APIRequestSaveListResult = "/api/Request/SaveListResult/{id}";

	public const string APIRequestForDelivery = "/api/Request/GetForDeliverys/{id}";

	public const string APIRequestUpdateFile = "/api/Request/UpdateFile/{id}";

	public const string APIRequestDownloadFile = "/api/Request/DownloadFile/{id}";

	public const string APIRequestCpk = "/api/Request/GetCpks/{id}";

	public const string APIComment = "/api/Comment/Gets";

	public const string APICommentDelete = "/api/Comment/Delete/{id}";

	public const string APICommentSave = "/api/Comment/Save";

	public const string APICommentUpdateFile = "/api/Comment/UpdateFile/{id}";

	public const string APICommentDownloadFile = "/api/Comment/DownloadFile/{id}";

	public const string APITemplateProduct = "/api/Template/GetProductTemplates/{id}";

	public const string APITemplateProductAll = "/api/Template/GetAllProductTemplates/{id}";

	public const string APITemplatePlan = "/api/Template/GetPlanTemplates/{id}";

	public const string APITemplate = "/api/Template/Gets";

	public const string APITemplateDelete = "/api/Template/Delete/{id}";

	public const string APITemplateSave = "/api/Template/Save";

	public const string APITemplateSaveList = "/api/Template/SaveList";

	public const string APITemplateUpdateExcel = "/api/Template/UpdateExcel/{id}";

	public const string APITemplateDownloadExcel = "/api/Template/DownloadExcel/{id}";

	public const string APITemplateExport = "/api/Template/Export";

	public const string APITemplateExportAllOnFile = "/api/Template/ExportAllOnFile";

	public const string APITemplateExportForProduct = "/api/Template/ExportForProduct";

	public const string APITemplateExportExcel = "/api/Template/ExportExcel";

	public const string APITemplateExportExcelChart = "/api/Template/ExportExcelChart/{id}";

	public const string APITemplateExportExcelSimple = "/api/Template/ExportExcelSimple";

	public const string APITemplateExportExcelSpecial = "/api/Template/ExportExcelSpecial/{id}";

	public const string APITemplateExportForDelivery = "/api/Template/ExportForDelivery";

	public const string APITemplateExportOneForDelivery = "/api/Template/ExportOneForDelivery";

	public const string APIPlanForProcess = "/api/Plan/GetProcesss/{id}/{sample}";

	public const string APIPlan = "/api/Plan/Gets";

	public const string APIPlanDelete = "/api/Plan/Delete/{id}";

	public const string APIPlanSave = "/api/Plan/Save";

	public const string APIPlanDetailForRequest = "/api/PlanDetail/Gets/{productid}/{id}";

	public const string APIPlanDetail = "/api/PlanDetail/Gets";

	public const string APIPlanDetailDelete = "/api/PlanDetail/Delete/{id}";

	public const string APIPlanDetailSave = "/api/PlanDetail/Save";

	public const string APIPlanMove = "/api/Plan/Move/{idfrom}/{idto}";

	public const string APIStage = "/api/Stage/Gets";

	public const string APIStageDelete = "/api/Stage/Delete/{id}";

	public const string APIStageSave = "/api/Stage/Save";

	public const string APIStageSaveList = "/api/Stage/SaveList";

	public const string APIRequestStatusGets = "/api/RequestStatus/Gets";

	public const string APIRequestStatus = "/api/RequestStatus/Gets/{id}";

	public const string APIRequestStatusSaveListWithId = "/api/RequestStatus/SaveListWithId";

	public const string APIConstant = "/api/Constant/Gets";

	public const string APIStatisticProductNGForDates = "/api/Statistic/GetProductNGForDates";

	public const string APIStatisticResultNGForDates = "/api/Statistic/GetResultNGForDates";

	public const string APIStatisticTypeNGForDates = "/api/Statistic/GetTypeNGForDates";

	public const string APIStatisticResultNGForOneDates = "/api/Statistic/GetResultNGForOneDates";

	public const string APIStatisticDetailNGForOneDates = "/api/Statistic/GetDetailNGForOneDates";

	public const string APIStatisticTypeNGForOneDates = "/api/Statistic/GetTypeNGForOneDates";

	public const string APIStatisticProductNGForWeeklys = "/api/Statistic/GetProductNGForWeeklys";

	public const string APIStatisticResultNGForWeeklys = "/api/Statistic/GetResultNGForWeeklys";

	public const string APIStatisticTypeNGForWeeklys = "/api/Statistic/GetTypeNGForWeeklys";

	public const string APIStatisticResultNGForOneWeeklys = "/api/Statistic/GetResultNGForOneWeeklys";

	public const string APIStatisticDetailNGForOneWeeklys = "/api/Statistic/GetDetailNGForOneWeeklys";

	public const string APIStatisticTypeNGForOneWeeklys = "/api/Statistic/GetTypeNGForOneWeeklys";

	public const string APIStatisticProductNGForMonths = "/api/Statistic/GetProductNGForMonths";

	public const string APIStatisticResultNGForMonths = "/api/Statistic/GetResultNGForMonths";

	public const string APIStatisticTypeNGForMonths = "/api/Statistic/GetTypeNGForMonths";

	public const string APIStatisticResultNGForOneMonths = "/api/Statistic/GetResultNGForOneMonths";

	public const string APIStatisticDetailNGForOneMonths = "/api/Statistic/GetDetailNGForOneMonths";

	public const string APIStatisticTypeNGForOneMonths = "/api/Statistic/GetTypeNGForOneMonths";

	public const string APISimilar = "/api/Similar/Gets/{id}";

	public const string APISimilarSave = "/api/Similar/Save";

	public const string APISimilarDelete = "/api/Similar/Delete";

	public const string APITemplateOther = "/api/TemplateOther/Gets";

	public const string APITemplateOtherDelete = "/api/TemplateOther/Delete/{id}";

	public const string APITemplateOtherSave = "/api/TemplateOther/Save";

	public const string APICalibration = "/api/Calibration/Gets";

	public const string APICalibrationDelete = "/api/Calibration/Delete/{id}";

	public const string APICalibrationSave = "/api/Calibration/Save";

	public const string APICalibrationUpdateFile = "/api/Calibration/UpdateFile/{id}";

	public const string APICalibrationDownloadFile = "/api/Calibration/DownloadFile/{id}";

	public const string APIEmail = "/api/Email/Gets";

	public const string APIEmailSave = "/api/Email/Save";

	public const string APISetting = "/api/Setting/Gets";

	public const string APISettingSave = "/api/Setting/Save";

	public const string APISettingSaveList = "/api/Setting/SaveList";

	public const string APIAQL = "/api/AQL/Gets";

	public const string APIAQLDelete = "/api/AQL/Delete/{id}";

	public const string APIAQLSave = "/api/AQL/Save";

	public const string APIAQLMove = "/api/AQL/Move/{idfrom}/{idto}";

	public const string APIAQLSaveList = "/api/AQL/SaveList";

	public const string APIAQLDeleteList = "/api/AQL/DeleteList";

	public const string APIAQLSamples = "/api/AQL/Samples";

	public const string APIResultRank = "/api/ResultRank/Gets";

	public const string APIProductGroup = "/api/ProductGroup/Gets";

	public const string APIProductGroupDelete = "/api/ProductGroup/Delete/{id}";

	public const string APIProductGroupSave = "/api/ProductGroup/Save";

	public const string APIProductGroupCopy = "/api/ProductGroup/Copy/{id}";
}
