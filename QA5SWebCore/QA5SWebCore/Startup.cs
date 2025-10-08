using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QA5SWebCore.Data;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Middleware;
using QA5SWebCore.Services;
using QA5SWebCore.Sockets;
using QA5SWebCore.ViewModels;
using QA5SWebCore.ViewModels.ProfileMapper;
using System;
using System.Text;

namespace QA5SWebCore;

public class Startup
{
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCors(delegate(CorsOptions o)
		{
			o.AddPolicy("ConnectorPolicy", delegate(CorsPolicyBuilder builder)
			{
				builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
			});
		});
		services.AddWebSocketManager();
		services.AddControllersWithViews();
		services.AddDbContext<RepositoryContext>(delegate(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		});
		services.AddScoped<IRepositoryFactory, UnitOfWork<RepositoryContext>>();
		services.AddScoped<IUnitOfWork, UnitOfWork<RepositoryContext>>();
		services.AddScoped<IUnitOfWork<RepositoryContext>, UnitOfWork<RepositoryContext>>();
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
		
		// Add AutoMapper with modern DI approach
		services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
		
		RegisterServices(services);
		services.AddTransient<TokenManagerMiddleware>();
		services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		services.AddMvc(delegate(MvcOptions option)
		{
			option.EnableEndpointRouting = false;
		}).AddNewtonsoftJson(delegate(MvcNewtonsoftJsonOptions options)
		{
			options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
		});
		services.AddAuthentication("Bearer").AddJwtBearer(delegate(JwtBearerOptions cfg)
		{
			cfg.RequireHttpsMetadata = false;
			cfg.SaveToken = true;
			cfg.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateLifetime = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A")),
				RequireExpirationTime = true,
				ClockSkew = TimeSpan.Zero,
				ValidateIssuer = false,
				ValidateAudience = false
			};
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RepositoryContext ctx, IServiceProvider serviceProvider)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}
		app.UseCors("ConnectorPolicy");
		app.UseWebSockets();
		app.MapSockets("/ws", serviceProvider.GetService<WebSocketMessageHandler>());
		//app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseAuthentication();
		app.UseMiddleware<TokenManagerMiddleware>(Array.Empty<object>());
		app.UseMvc(delegate(IRouteBuilder routes)
		{
			routes.MapRoute("default", "{controller}/{action=Index}/{id}");
		});
		
		// Remove the static Mapper.Initialize call completely
		
		DBInitializer.Initialize(ctx, Configuration);
	}

	private void RegisterServices(IServiceCollection services)
	{
		services.AddScoped<IAuthUserService, AuthUserService>();
		services.AddScoped<IMachineService, MachineService>();
		services.AddScoped<IMeasurementService, MeasurementService>();
		services.AddScoped<IMetadataValueService, MetadataValueService>();
		services.AddScoped<IProductService, ProductService>();
		services.AddScoped<ITemplateService, TemplateService>();
		services.AddScoped<ITokenManager, TokenManagerService>();
		services.AddScoped<IToolService, ToolService>();
		services.AddScoped<IRequestResultService, RequestResultService>();
		services.AddScoped<IRequestService, RequestService>();
		services.AddScoped<ICommentService, CommentService>();
		services.AddScoped<IPlanService, PlanService>();
		services.AddScoped<IPlanDetailService, PlanDetailService>();
		services.AddScoped<IRequestStatusService, RequestStatusService>();
		services.AddScoped<IConstantService, ConstantService>();
		services.AddScoped<IEmailService, EmailService>();
		services.AddScoped<IStatisticService, StatisticService>();
		services.AddScoped<ISimilarService, SimilarService>();
		services.AddScoped<ITemplateOtherService, TemplateOtherService>();
		services.AddScoped<ICalibrationService, CalibrationService>();
		services.AddScoped<IResultFileService, ResultFileService>();
		services.AddScoped<ISettingService, SettingService>();
		services.AddScoped<IAQLService, AQLService>();
		services.AddScoped<IResultRankService, ResultRankService>();
		services.AddScoped<IProductGroupService, ProductGroupService>();
	}
}
