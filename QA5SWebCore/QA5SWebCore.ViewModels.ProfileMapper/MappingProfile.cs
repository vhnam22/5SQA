using System;
using System.Linq;
using AutoMapper;
using QA5SWebCore.Models;

namespace QA5SWebCore.ViewModels.ProfileMapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<MetadataTypeViewModel, MetadataType>();
		CreateMap<MetadataType, MetadataTypeViewModel>().ForMember((MetadataTypeViewModel m) => m.ParentName, delegate(IMemberConfigurationExpression<MetadataType, MetadataTypeViewModel, string> opt)
		{
			opt.MapFrom((MetadataType src) => src.Parent.Name);
		}).ForMember((MetadataTypeViewModel m) => m.ParentCode, delegate(IMemberConfigurationExpression<MetadataType, MetadataTypeViewModel, string> opt)
		{
			opt.MapFrom((MetadataType src) => src.Parent.Code);
		});
		CreateMap<MetadataValueViewModel, MetadataValue>();
		CreateMap<MetadataValue, MetadataValueViewModel>().ForMember((MetadataValueViewModel m) => m.TypeName, delegate(IMemberConfigurationExpression<MetadataValue, MetadataValueViewModel, string> opt)
		{
			opt.MapFrom((MetadataValue src) => src.Type.Name);
		}).ForMember((MetadataValueViewModel m) => m.TypeCode, delegate(IMemberConfigurationExpression<MetadataValue, MetadataValueViewModel, string> opt)
		{
			opt.MapFrom((MetadataValue src) => src.Type.Code);
		}).ForMember((MetadataValueViewModel m) => m.ParentName, delegate(IMemberConfigurationExpression<MetadataValue, MetadataValueViewModel, string> opt)
		{
			opt.MapFrom((MetadataValue src) => src.Parent.Name);
		})
			.ForMember((MetadataValueViewModel m) => m.ParentCode, delegate(IMemberConfigurationExpression<MetadataValue, MetadataValueViewModel, string> opt)
			{
				opt.MapFrom((MetadataValue src) => src.Parent.Code);
			});
		
		CreateMap<AuthUserViewModel, AuthUser>();
		CreateMap<AuthUser, AuthUserViewModel>().ForMember((AuthUserViewModel m) => m.DepartmentName, delegate(IMemberConfigurationExpression<AuthUser, AuthUserViewModel, string> opt)
		{
			opt.MapFrom((AuthUser src) => src.Department.Name);
		});
		CreateMap<MachineViewModel, Machine>();
		CreateMap<Machine, MachineViewModel>().ForMember((MachineViewModel m) => m.ExpDate, delegate(IMemberConfigurationExpression<Machine, MachineViewModel, DateTimeOffset?> opt)
		{
			opt.MapFrom((Machine src) => src.Calibrations.OrderByDescending((Calibration x) => x.Created).FirstOrDefault().ExpDate);
		}).ForMember((MachineViewModel m) => m.FactoryName, delegate(IMemberConfigurationExpression<Machine, MachineViewModel, string> opt)
		{
			opt.MapFrom((Machine src) => src.Factory.Name);
		}).ForMember((MachineViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Machine, MachineViewModel, string> opt)
		{
			opt.MapFrom((Machine src) => src.MachineType.Name);
		});
		CreateMap<Machine, MachineForToolViewModel>();
		CreateMap<TemplateViewModel, Template>().ReverseMap();
		CreateMap<CommentViewModel, Comment>().ReverseMap();
		CreateMap<ToolViewModel, Tool>();
		CreateMap<Tool, ToolViewModel>().ForMember((ToolViewModel m) => m.TabletCode, delegate(IMemberConfigurationExpression<Tool, ToolViewModel, string> opt)
		{
			opt.MapFrom((Tool src) => src.Tablet.Code);
		}).ForMember((ToolViewModel m) => m.TabletName, delegate(IMemberConfigurationExpression<Tool, ToolViewModel, string> opt)
		{
			opt.MapFrom((Tool src) => src.Tablet.Name);
		}).ForMember((ToolViewModel m) => m.MachineCode, delegate(IMemberConfigurationExpression<Tool, ToolViewModel, string> opt)
		{
			opt.MapFrom((Tool src) => src.Machine.Code);
		})
			.ForMember((ToolViewModel m) => m.MachineName, delegate(IMemberConfigurationExpression<Tool, ToolViewModel, string> opt)
			{
				opt.MapFrom((Tool src) => src.Machine.Name);
			})
			.ForMember((ToolViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Tool, ToolViewModel, string> opt)
			{
				opt.MapFrom((Tool src) => src.Machine.MachineType.Name);
			});
		CreateMap<Tool, ToolResultViewModel>().ForMember((ToolResultViewModel m) => m.MachineName, delegate(IMemberConfigurationExpression<Tool, ToolResultViewModel, string> opt)
		{
			opt.MapFrom((Tool src) => src.Machine.Name);
		}).ForMember((ToolResultViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Tool, ToolResultViewModel, string> opt)
		{
			opt.MapFrom((Tool src) => src.Machine.MachineType.Name);
		});
		CreateMap<ProductViewModel, Product>();
		CreateMap<Product, ProductViewModel>()
		.ForMember((ProductViewModel m) => m.CodeName, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => $"{src.Code} ({src.Name})");
		})
		.ForMember((ProductViewModel m) => m.TemplateName, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => src.Template.Name);
		})
		.ForMember((ProductViewModel m) => m.DepartmentName, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => src.Department.Name);
		})
		.ForMember((ProductViewModel m) => m.GroupCode, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => src.Group.Code);
		})
		.ForMember((ProductViewModel m) => m.GroupName, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => src.Group.Name);
		})
		.ForMember((ProductViewModel m) => m.GroupCodeName, delegate (IMemberConfigurationExpression<Product, ProductViewModel, string> opt)
		{
			opt.MapFrom((Product src) => $"{src.Group.Code} ({src.Group.Name})");
		})
        .ForMember((ProductViewModel m) => m.TotalMeas, delegate(IMemberConfigurationExpression<Product, ProductViewModel, int?> opt)
		{
			opt.MapFrom((Product src) => src.Measurements.Count);
		})
        .ForMember((ProductViewModel m) => m.IsAQL, delegate(IMemberConfigurationExpression<Product, ProductViewModel, bool?> opt)
		{
			opt.MapFrom((Product src) => src.AQLs.Count > 0);
		});
        CreateMap<MeasurementViewModel, Measurement>();
		CreateMap<Measurement, MeasurementViewModel>().ForMember((MeasurementViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Measurement, MeasurementViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.MachineType.Name);
		}).ForMember((MeasurementViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<Measurement, MeasurementViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Important.Name);
		}).ForMember((MeasurementViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<Measurement, MeasurementViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Product.Name);
		})
			.ForMember((MeasurementViewModel m) => m.UnitName, delegate(IMemberConfigurationExpression<Measurement, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			})
			.ForMember((MeasurementViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<Measurement, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Template.Name);
			});
		CreateMap<Measurement, MeasurementPlanViewModel>().ForMember((MeasurementPlanViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Measurement, MeasurementPlanViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.MachineType.Name);
		}).ForMember((MeasurementPlanViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<Measurement, MeasurementPlanViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Important.Name);
		}).ForMember((MeasurementPlanViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<Measurement, MeasurementPlanViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Product.Name);
		})
			.ForMember((MeasurementPlanViewModel m) => m.UnitName, delegate(IMemberConfigurationExpression<Measurement, MeasurementPlanViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			})
			.ForMember((MeasurementPlanViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<Measurement, MeasurementPlanViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Template.Name);
			});
		CreateMap<Measurement, ChartViewModel>().ForMember((ChartViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.MachineType.Name);
		}).ForMember((ChartViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Important.Name);
		}).ForMember((ChartViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Product.Code);
		})
			.ForMember((ChartViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Product.Name);
			})
			.ForMember((ChartViewModel m) => m.UnitName, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			})
			.ForMember((ChartViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<Measurement, ChartViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Template.Name);
			});
		CreateMap<Measurement, RequestResultViewModel>().ForMember((RequestResultViewModel m) => m.MeasurementId, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, Guid?> opt)
		{
			opt.MapFrom((Measurement src) => src.Id);
		}).ForMember((RequestResultViewModel m) => m.MeasurementCode, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Code);
		}).ForMember((RequestResultViewModel m) => m.MeasurementName, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Name);
		})
			.ForMember((RequestResultViewModel m) => m.MeasurementUnit, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			})
			.ForMember((RequestResultViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Important.Name);
			})
			.ForMember((RequestResultViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.MachineType.Name);
			})
			.ForMember((RequestResultViewModel m) => m.Unit, delegate(IMemberConfigurationExpression<Measurement, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			});
		CreateMap<Measurement, ResultFullViewModel>().ForMember((ResultFullViewModel m) => m.MeasurementId, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, Guid?> opt)
		{
			opt.MapFrom((Measurement src) => src.Id);
		}).ForMember((ResultFullViewModel m) => m.MeasurementCode, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Code);
		}).ForMember((ResultFullViewModel m) => m.MeasurementName, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Name);
		})
			.ForMember((ResultFullViewModel m) => m.MeasurementUnit, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			})
			.ForMember((ResultFullViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Important.Name);
			})
			.ForMember((ResultFullViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.MachineType.Name);
			})
			.ForMember((ResultFullViewModel m) => m.USL, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.UnitId.HasValue ? ((double?)Math.Round(double.Parse(src.Value) + src.Upper.Value, 5, MidpointRounding.AwayFromZero)) : ((double?)null));
			})
			.ForMember((ResultFullViewModel m) => m.LSL, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.UnitId.HasValue ? ((double?)Math.Round(double.Parse(src.Value) + src.Lower.Value, 5, MidpointRounding.AwayFromZero)) : ((double?)null));
			})
			.ForMember((ResultFullViewModel m) => m.Unit, delegate(IMemberConfigurationExpression<Measurement, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Unit.Name);
			});
		CreateMap<Measurement, FormulaViewModel>().ForMember((FormulaViewModel m) => m.MeasurementId, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, Guid?> opt)
		{
			opt.MapFrom((Measurement src) => src.Id);
		}).ForMember((FormulaViewModel m) => m.Unit, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Unit.Name);
		}).ForMember((FormulaViewModel m) => m.ValueOld, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, string> opt)
		{
			opt.MapFrom((Measurement src) => src.Value);
		})
			.ForMember((FormulaViewModel m) => m.ValueOrigin, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, string> opt)
			{
				opt.MapFrom((Measurement src) => src.Value);
			})
			.ForMember((FormulaViewModel m) => m.UpperOld, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.Upper);
			})
			.ForMember((FormulaViewModel m) => m.UpperOrigin, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.Upper);
			})
			.ForMember((FormulaViewModel m) => m.LowerOld, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.Lower);
			})
			.ForMember((FormulaViewModel m) => m.LowerOrigin, delegate(IMemberConfigurationExpression<Measurement, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((Measurement src) => src.Lower);
			});
		CreateMap<RequestViewModel, Request>();
		CreateMap<Request, RequestViewModel>().ForMember((RequestViewModel m) => m.GroupId, delegate(IMemberConfigurationExpression<Request, RequestViewModel, Guid?> opt)
		{
			opt.MapFrom((Request src) => src.Product.GroupId);
		}).ForMember((RequestViewModel m) => m.ProductStage, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
		{
			opt.MapFrom((Request src) => src.Product.Name);
		}).ForMember((RequestViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
		{
			opt.MapFrom((Request src) => src.Product.Group.Code);
		})
			.ForMember((RequestViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
			{
				opt.MapFrom((Request src) => src.Product.Group.Name);
			})
			.ForMember((RequestViewModel m) => m.ProductDescription, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
			{
				opt.MapFrom((Request src) => src.Product.Group.Description);
			})
			.ForMember((RequestViewModel m) => m.ProductDepartment, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
			{
				opt.MapFrom((Request src) => src.Product.Department.Name);
			})
			.ForMember((RequestViewModel m) => m.ProductImageUrl, delegate(IMemberConfigurationExpression<Request, RequestViewModel, string> opt)
			{
				opt.MapFrom((Request src) => src.Product.ImageUrl);
			})
			.ForMember((RequestViewModel m) => m.ProductCavity, delegate(IMemberConfigurationExpression<Request, RequestViewModel, int?> opt)
			{
				opt.MapFrom((Request src) => src.Product.Cavity);
			})
			.ForMember((RequestViewModel m) => m.TotalComment, delegate(IMemberConfigurationExpression<Request, RequestViewModel, int?> opt)
			{
				opt.MapFrom((Request src) => src.Comments.Count);
			});
		CreateMap<RequestStatusViewModel, RequestStatus>();
		CreateMap<RequestStatus, RequestStatusViewModel>().ForMember((RequestStatusViewModel m) => m.RequestName, delegate(IMemberConfigurationExpression<RequestStatus, RequestStatusViewModel, string> opt)
		{
			opt.MapFrom((RequestStatus src) => src.Request.Name);
		}).ForMember((RequestStatusViewModel m) => m.PlanName, delegate(IMemberConfigurationExpression<RequestStatus, RequestStatusViewModel, string> opt)
		{
			opt.MapFrom((RequestStatus src) => src.Plan.Stage.Name);
		});
		CreateMap<FormulaViewModel, RequestResult>();
		CreateMap<RequestResult, FormulaViewModel>().ForMember((FormulaViewModel m) => m.Code, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Measurement.Code);
		}).ForMember((FormulaViewModel m) => m.Name, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Measurement.Name);
		}).ForMember((FormulaViewModel m) => m.ValueOld, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Value);
		})
			.ForMember((FormulaViewModel m) => m.ValueOrigin, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Value);
			})
			.ForMember((FormulaViewModel m) => m.UpperOrigin, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Upper);
			})
			.ForMember((FormulaViewModel m) => m.UpperOld, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Upper);
			})
			.ForMember((FormulaViewModel m) => m.LowerOrigin, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Lower);
			})
			.ForMember((FormulaViewModel m) => m.LowerOld, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Lower);
			})
			.ForMember((FormulaViewModel m) => m.Unit, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Unit.Name);
			})
			.ForMember((FormulaViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Formula);
			})
			.ForMember((FormulaViewModel m) => m.MachineNameOld, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.MachineName);
			})
			.ForMember((FormulaViewModel m) => m.ResultOld, delegate(IMemberConfigurationExpression<RequestResult, FormulaViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Result);
			});
		CreateMap<RequestResultViewModel, RequestResult>();
		CreateMap<RequestResult, RequestResultViewModel>().ForMember((RequestResultViewModel m) => m.RequestName, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Name);
		}).ForMember((RequestResultViewModel m) => m.MeasurementCode, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Measurement.Code);
		}).ForMember((RequestResultViewModel m) => m.MeasurementName, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Measurement.Name);
		})
			.ForMember((RequestResultViewModel m) => m.MeasurementUnit, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Unit.Name);
			})
			.ForMember((RequestResultViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Important.Name);
			})
			.ForMember((RequestResultViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Formula);
			})
			.ForMember((RequestResultViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.MachineType.Name);
			})
			.ForMember((RequestResultViewModel m) => m.ProductSort, delegate(IMemberConfigurationExpression<RequestResult, RequestResultViewModel, int?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Product.Sort);
			});
		CreateMap<RequestResult, ResultFullViewModel>().ForMember((ResultFullViewModel m) => m.ProductId, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, Guid?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.ProductId);
		}).ForMember((ResultFullViewModel m) => m.RequestDate, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, DateTimeOffset?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Date);
		}).ForMember((ResultFullViewModel m) => m.RequestName, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Name);
		})
			.ForMember((ResultFullViewModel m) => m.RequestSample, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Sample);
			})
			.ForMember((ResultFullViewModel m) => m.MeasurementCode, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Code);
			})
			.ForMember((ResultFullViewModel m) => m.MeasurementName, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Name);
			})
			.ForMember((ResultFullViewModel m) => m.MeasurementUnit, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Unit.Name);
			})
			.ForMember((ResultFullViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Important.Name);
			})
			.ForMember((ResultFullViewModel m) => m.USL, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => string.IsNullOrEmpty(src.Unit) ? ((double?)null) : ((double?)Math.Round(double.Parse(src.Value) + src.Upper.Value, 5, MidpointRounding.AwayFromZero)));
			})
			.ForMember((ResultFullViewModel m) => m.LSL, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => string.IsNullOrEmpty(src.Unit) ? ((double?)null) : ((double?)Math.Round(double.Parse(src.Value) + src.Lower.Value, 5, MidpointRounding.AwayFromZero)));
			})
			.ForMember((ResultFullViewModel m) => m.UCL, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.UCL);
			})
			.ForMember((ResultFullViewModel m) => m.LCL, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, double?> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.LCL);
			})
			.ForMember((ResultFullViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Formula);
			})
			.ForMember((ResultFullViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.MachineType.Name);
			})
			.ForMember((ResultFullViewModel m) => m.Coordinate, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Measurement.Coordinate);
			})
			.ForMember((ResultFullViewModel m) => m.Lot, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Lot);
			})
			.ForMember((ResultFullViewModel m) => m.Line, delegate(IMemberConfigurationExpression<RequestResult, ResultFullViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Line);
			});
		CreateMap<RequestResult, StatisticResultViewModel>().ForMember((StatisticResultViewModel m) => m.Date, delegate(IMemberConfigurationExpression<RequestResult, StatisticResultViewModel, DateTimeOffset?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Date);
		}).ForMember((StatisticResultViewModel m) => m.RequestName, delegate(IMemberConfigurationExpression<RequestResult, StatisticResultViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Name);
		}).ForMember((StatisticResultViewModel m) => m.Lot, delegate(IMemberConfigurationExpression<RequestResult, StatisticResultViewModel, string> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Lot);
		})
			.ForMember((StatisticResultViewModel m) => m.Line, delegate(IMemberConfigurationExpression<RequestResult, StatisticResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Line);
			})
			.ForMember((StatisticResultViewModel m) => m.Type, delegate(IMemberConfigurationExpression<RequestResult, StatisticResultViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Type);
			});
		CreateMap<PlanViewModel, Plan>();
		CreateMap<Plan, PlanViewModel>().ForMember((PlanViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<Plan, PlanViewModel, string> opt)
		{
			opt.MapFrom((Plan src) => src.Product.Code);
		}).ForMember((PlanViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<Plan, PlanViewModel, string> opt)
		{
			opt.MapFrom((Plan src) => src.Product.Name);
		}).ForMember((PlanViewModel m) => m.StageName, delegate(IMemberConfigurationExpression<Plan, PlanViewModel, string> opt)
		{
			opt.MapFrom((Plan src) => src.Stage.Name);
		})
			.ForMember((PlanViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<Plan, PlanViewModel, string> opt)
			{
				opt.MapFrom((Plan src) => src.Template.Name);
			})
			.ForMember((PlanViewModel m) => m.TotalDetails, delegate(IMemberConfigurationExpression<Plan, PlanViewModel, int?> opt)
			{
				opt.MapFrom((Plan src) => src.PlanDetails.Count);
			});
		CreateMap<PlanDetailViewModel, PlanDetail>();
		CreateMap<PlanDetail, PlanDetailViewModel>().ForMember((PlanDetailViewModel m) => m.PlanName, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => $"{src.Plan.Product.Code}-{src.Plan.Stage.Name}");
		}).ForMember((PlanDetailViewModel m) => m.Code, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Code);
		}).ForMember((PlanDetailViewModel m) => m.Name, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Name);
		})
			.ForMember((PlanDetailViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Important.Name);
			})
			.ForMember((PlanDetailViewModel m) => m.Value, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Value);
			})
			.ForMember((PlanDetailViewModel m) => m.Upper, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Upper);
			})
			.ForMember((PlanDetailViewModel m) => m.Lower, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Lower);
			})
			.ForMember((PlanDetailViewModel m) => m.UnitName, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Unit.Name);
			})
			.ForMember((PlanDetailViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.MachineType.Name);
			})
			.ForMember((PlanDetailViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Formula);
			})
			.ForMember((PlanDetailViewModel m) => m.TemplateCode, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Template.Code);
			})
			.ForMember((PlanDetailViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Template.Name);
			})
			.ForMember((PlanDetailViewModel m) => m.Sort, delegate(IMemberConfigurationExpression<PlanDetail, PlanDetailViewModel, int?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Sort);
			});
		CreateMap<PlanDetail, MeasurementViewModel>().ForMember((MeasurementViewModel m) => m.Code, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Code);
		}).ForMember((MeasurementViewModel m) => m.Name, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Name);
		}).ForMember((MeasurementViewModel m) => m.ImportantId, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, Guid?> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.ImportantId);
		})
			.ForMember((MeasurementViewModel m) => m.ImportantName, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Important.Name);
			})
			.ForMember((MeasurementViewModel m) => m.Value, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Value);
			})
			.ForMember((MeasurementViewModel m) => m.Upper, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Upper);
			})
			.ForMember((MeasurementViewModel m) => m.Lower, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Lower);
			})
			.ForMember((MeasurementViewModel m) => m.UnitId, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, Guid?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.UnitId);
			})
			.ForMember((MeasurementViewModel m) => m.UnitName, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Unit.Name);
			})
			.ForMember((MeasurementViewModel m) => m.MachineTypeId, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, Guid?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.MachineTypeId);
			})
			.ForMember((MeasurementViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.MachineType.Name);
			})
			.ForMember((MeasurementViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Formula);
			})
			.ForMember((MeasurementViewModel m) => m.TemplateId, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, Guid?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.TemplateId);
			})
			.ForMember((MeasurementViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Template.Name);
			})
			.ForMember((MeasurementViewModel m) => m.Sort, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, int?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Sort);
			})
			.ForMember((MeasurementViewModel m) => m.ProductId, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, Guid?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.ProductId);
			})
			.ForMember((MeasurementViewModel m) => m.ProductName, delegate(IMemberConfigurationExpression<PlanDetail, MeasurementViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Product.Name);
			});
		CreateMap<PlanDetail, RequestResultViewModel>().ForMember((RequestResultViewModel m) => m.MeasurementCode, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Code);
		}).ForMember((RequestResultViewModel m) => m.Name, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Name);
		}).ForMember((RequestResultViewModel m) => m.Value, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
		{
			opt.MapFrom((PlanDetail src) => src.Measurement.Value);
		})
			.ForMember((RequestResultViewModel m) => m.Upper, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Upper);
			})
			.ForMember((RequestResultViewModel m) => m.Lower, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, double?> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Lower);
			})
			.ForMember((RequestResultViewModel m) => m.Unit, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Unit.Name);
			})
			.ForMember((RequestResultViewModel m) => m.MachineTypeName, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.MachineType.Name);
			})
			.ForMember((RequestResultViewModel m) => m.Formula, delegate(IMemberConfigurationExpression<PlanDetail, RequestResultViewModel, string> opt)
			{
				opt.MapFrom((PlanDetail src) => src.Measurement.Formula);
			});
		CreateMap<RequestResult, ResultStatisticViewModel>().ForMember((ResultStatisticViewModel m) => m.RequestDate, delegate(IMemberConfigurationExpression<RequestResult, ResultStatisticViewModel, DateTimeOffset?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Date);
		}).ForMember((ResultStatisticViewModel m) => m.RequestSample, delegate(IMemberConfigurationExpression<RequestResult, ResultStatisticViewModel, int?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.Sample);
		}).ForMember((ResultStatisticViewModel m) => m.ProductId, delegate(IMemberConfigurationExpression<RequestResult, ResultStatisticViewModel, Guid?> opt)
		{
			opt.MapFrom((RequestResult src) => src.Request.ProductId);
		})
			.ForMember((ResultStatisticViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<RequestResult, ResultStatisticViewModel, string> opt)
			{
				opt.MapFrom((RequestResult src) => src.Request.Product.Code);
			});
		CreateMap<RequestStatisticViewModel, RequestStatistic>();
		CreateMap<RequestStatistic, RequestStatisticViewModel>().ForMember((RequestStatisticViewModel m) => m.RequestDate, delegate(IMemberConfigurationExpression<RequestStatistic, RequestStatisticViewModel, DateTimeOffset?> opt)
		{
			opt.MapFrom((RequestStatistic src) => src.Request.Date);
		}).ForMember((RequestStatisticViewModel m) => m.RequestSample, delegate(IMemberConfigurationExpression<RequestStatistic, RequestStatisticViewModel, int?> opt)
		{
			opt.MapFrom((RequestStatistic src) => src.Request.Sample);
		}).ForMember((RequestStatisticViewModel m) => m.RequestType, delegate(IMemberConfigurationExpression<RequestStatistic, RequestStatisticViewModel, string> opt)
		{
			opt.MapFrom((RequestStatistic src) => src.Request.Type);
		})
			.ForMember((RequestStatisticViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<RequestStatistic, RequestStatisticViewModel, string> opt)
			{
				opt.MapFrom((RequestStatistic src) => $"{src.Request.Product.Group.Code}#{src.Request.Product.Name}");
			})
			.ForMember((RequestStatisticViewModel m) => m.ProductCavity, delegate(IMemberConfigurationExpression<RequestStatistic, RequestStatisticViewModel, int?> opt)
			{
				opt.MapFrom((RequestStatistic src) => src.Request.Product.Cavity);
			});
		CreateMap<RequestResultStatisticViewModel, RequestResultStatistic>();
		CreateMap<RequestResultStatistic, RequestResultStatisticViewModel>().ForMember((RequestResultStatisticViewModel m) => m.RequestType, delegate(IMemberConfigurationExpression<RequestResultStatistic, RequestResultStatisticViewModel, string> opt)
		{
			opt.MapFrom((RequestResultStatistic src) => src.Request.Type);
		}).ForMember((RequestResultStatisticViewModel m) => m.ProductCode, delegate(IMemberConfigurationExpression<RequestResultStatistic, RequestResultStatisticViewModel, string> opt)
		{
			opt.MapFrom((RequestResultStatistic src) => $"{src.Request.Product.Group.Code}#{src.Request.Product.Name}");
		}).ForMember((RequestResultStatisticViewModel m) => m.MeasurementName, delegate(IMemberConfigurationExpression<RequestResultStatistic, RequestResultStatisticViewModel, string> opt)
		{
			opt.MapFrom((RequestResultStatistic src) => src.Measurement.Name);
		});
		CreateMap<SimilarViewModel, Similar>().ReverseMap();
		CreateMap<EmailViewModel, Email>().ReverseMap();
		CreateMap<SettingViewModel, Setting>().ReverseMap();
		CreateMap<TemplateOtherViewModel, TemplateOther>();
		CreateMap<TemplateOther, TemplateOtherViewModel>().ForMember((TemplateOtherViewModel m) => m.TemplateName, delegate(IMemberConfigurationExpression<TemplateOther, TemplateOtherViewModel, string> opt)
		{
			opt.MapFrom((TemplateOther src) => src.Template.Name);
		}).ForMember((TemplateOtherViewModel m) => m.TemplateDescription, delegate(IMemberConfigurationExpression<TemplateOther, TemplateOtherViewModel, string> opt)
		{
			opt.MapFrom((TemplateOther src) => src.Template.Description);
		});
		CreateMap<CalibrationViewModel, Calibration>();
		CreateMap<Calibration, CalibrationViewModel>().ForMember((CalibrationViewModel dest) => dest.TypeName, delegate(IMemberConfigurationExpression<Calibration, CalibrationViewModel, string> otp)
		{
			otp.MapFrom((Calibration src) => src.Type.Name);
		}).ForMember((CalibrationViewModel dest) => dest.MachineModel, delegate(IMemberConfigurationExpression<Calibration, CalibrationViewModel, string> otp)
		{
			otp.MapFrom((Calibration src) => src.Machine.Model);
		}).ForMember((CalibrationViewModel dest) => dest.MachineName, delegate(IMemberConfigurationExpression<Calibration, CalibrationViewModel, string> otp)
		{
			otp.MapFrom((Calibration src) => src.Machine.Name);
		});
		CreateMap<ResultFileViewModel, ResultFile>();
		CreateMap<ResultFile, ResultFileViewModel>().ForMember((ResultFileViewModel dest) => dest.RequestName, delegate(IMemberConfigurationExpression<ResultFile, ResultFileViewModel, string> otp)
		{
			otp.MapFrom((ResultFile src) => src.Request.Name);
		});
		CreateMap<AQLViewModel, AQL>();
		CreateMap<AQL, AQLViewModel>().ForMember((AQLViewModel dest) => dest.ProductCode, delegate(IMemberConfigurationExpression<AQL, AQLViewModel, string> otp)
		{
			otp.MapFrom((AQL src) => src.Product.Code);
		}).ForMember((AQLViewModel dest) => dest.ProductName, delegate(IMemberConfigurationExpression<AQL, AQLViewModel, string> otp)
		{
			otp.MapFrom((AQL src) => src.Product.Name);
		});
		CreateMap<ResultRankViewModel, ResultRank>();
		CreateMap<ResultRank, ResultRankViewModel>().ForMember((ResultRankViewModel dest) => dest.MeasurementName, delegate(IMemberConfigurationExpression<ResultRank, ResultRankViewModel, string> otp)
		{
			otp.MapFrom((ResultRank src) => src.Measurement.Name);
		});
		CreateMap<Machine, MachineLienceViewModel>().ForMember((MachineLienceViewModel dest) => dest.MachineType, delegate(IMemberConfigurationExpression<Machine, MachineLienceViewModel, string> otp)
		{
			otp.MapFrom((Machine src) => src.MachineType.Name);
		});
		CreateMap<ProductGroupViewModel, ProductGroup>();
		CreateMap<ProductGroup, ProductGroupViewModel>().ForMember((ProductGroupViewModel m) => m.CodeName, delegate(IMemberConfigurationExpression<ProductGroup, ProductGroupViewModel, string> opt)
		{
			opt.MapFrom((ProductGroup src) => $"{src.Code} ({src.Name})");
		}).ForMember((ProductGroupViewModel m) => m.ProductCount, delegate(IMemberConfigurationExpression<ProductGroup, ProductGroupViewModel, int?> opt)
		{
			opt.MapFrom((ProductGroup src) => src.Products.Count);
		});
	}
}
