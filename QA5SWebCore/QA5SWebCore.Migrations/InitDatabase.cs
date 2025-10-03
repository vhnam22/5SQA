using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using QA5SWebCore.Data;

namespace QA5SWebCore.Migrations;

[DbContext(typeof(RepositoryContext))]
[Migration("20211130060716_Init-Database")]
public class InitDatabase : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("MetadataTypes", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			Description = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ParentId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_MetadataTypes", x => x.Id);
			table.ForeignKey("FK_MetadataTypes_MetadataTypes_ParentId", x => x.ParentId, "MetadataTypes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("Templates", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			Description = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Limit = table.Column<int>(),
			Type = table.Column<string>(),
			TemplateUrl = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			TemplateData = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_Templates", x => x.Id);
		});
		migrationBuilder.CreateTable("MetadataValues", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			Description = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Value = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			TypeId = table.Column<Guid>(),
			ParentId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_MetadataValues", x => x.Id);
			table.ForeignKey("FK_MetadataValues_MetadataValues_ParentId", x => x.ParentId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_MetadataValues_MetadataTypes_TypeId", x => x.TypeId, "MetadataTypes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Products", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			Description = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ImageUrl = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Sample = table.Column<int>(),
			SampleMax = table.Column<int>(),
			TemplateId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_Products", x => x.Id);
			table.ForeignKey("FK_Products_Templates_TemplateId", x => x.TemplateId, "Templates", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("AuthUsers", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			FullName = table.Column<string>(),
			Email = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			BirthDate = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
			Gender = table.Column<string>(),
			PhoneNumber = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Address = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Position = table.Column<string>(),
			DepartmentId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			ImageUrl = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Username = table.Column<string>(),
			Password = table.Column<string>(),
			LoginAt = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			TimeLogin = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
			RefreshToken = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Role = table.Column<int>(null, null, null, rowVersion: false, null, nullable: false, 3)
		}, null, table =>
		{
			table.PrimaryKey("PK_AuthUsers", x => x.Id);
			table.ForeignKey("FK_AuthUsers_MetadataValues_DepartmentId", x => x.DepartmentId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("Machines", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			Model = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Serial = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Status = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			FactoryId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			MachineTypeId = table.Column<Guid>()
		}, null, table =>
		{
			table.PrimaryKey("PK_Machines", x => x.Id);
			table.ForeignKey("FK_Machines_MetadataValues_FactoryId", x => x.FactoryId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Machines_MetadataValues_MachineTypeId", x => x.MachineTypeId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Measurements", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			ImportantId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			Value = table.Column<string>(),
			Upper = table.Column<double>(null, null, null, rowVersion: false, null, nullable: true),
			Lower = table.Column<double>(null, null, null, rowVersion: false, null, nullable: true),
			UnitId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			MachineTypeId = table.Column<Guid>(),
			Formula = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			TemplateId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			Sort = table.Column<int>(),
			ProductId = table.Column<Guid>()
		}, null, table =>
		{
			table.PrimaryKey("PK_Measurements", x => x.Id);
			table.ForeignKey("FK_Measurements_MetadataValues_ImportantId", x => x.ImportantId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Measurements_MetadataValues_MachineTypeId", x => x.MachineTypeId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_Measurements_Products_ProductId", x => x.ProductId, "Products", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_Measurements_Templates_TemplateId", x => x.TemplateId, "Templates", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Measurements_MetadataValues_UnitId", x => x.UnitId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("Plans", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			ProductId = table.Column<Guid>(),
			TypeId = table.Column<Guid>(),
			TemplateId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_Plans", x => x.Id);
			table.ForeignKey("FK_Plans_Products_ProductId", x => x.ProductId, "Products", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_Plans_Templates_TemplateId", x => x.TemplateId, "Templates", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Plans_MetadataValues_TypeId", x => x.TypeId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Requests", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Code = table.Column<string>(),
			Name = table.Column<string>(),
			ProductId = table.Column<Guid>(),
			TypeId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			Date = table.Column<DateTimeOffset>(),
			LineId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			MoldId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			MachineMoldId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			PoNumber = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			PoQuantity = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Lot = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Supplier = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Sample = table.Column<int>(),
			CompletedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Completed = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
			CheckedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Checked = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
			ApprovedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Approved = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
			Judgement = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Status = table.Column<string>(),
			Special = table.Column<bool>()
		}, null, table =>
		{
			table.PrimaryKey("PK_Requests", x => x.Id);
			table.ForeignKey("FK_Requests_MetadataValues_LineId", x => x.LineId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Requests_MetadataValues_MachineMoldId", x => x.MachineMoldId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Requests_MetadataValues_MoldId", x => x.MoldId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Requests_Products_ProductId", x => x.ProductId, "Products", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_Requests_MetadataValues_TypeId", x => x.TypeId, "MetadataValues", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("Tools", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			TabletId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			MachineId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_Tools", x => x.Id);
			table.ForeignKey("FK_Tools_Machines_MachineId", x => x.MachineId, "Machines", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_Tools_Machines_TabletId", x => x.TabletId, "Machines", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
		});
		migrationBuilder.CreateTable("PlanDetails", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			PlanId = table.Column<Guid>(),
			MeasurementId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_PlanDetails", x => x.Id);
			table.ForeignKey("FK_PlanDetails_Measurements_MeasurementId", x => x.MeasurementId, "Measurements", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_PlanDetails_Plans_PlanId", x => x.PlanId, "Plans", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Comments", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			Content = table.Column<string>(),
			RequestId = table.Column<Guid>()
		}, null, table =>
		{
			table.PrimaryKey("PK_Comments", x => x.Id);
			table.ForeignKey("FK_Comments_Requests_RequestId", x => x.RequestId, "Requests", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("RequestResults", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>(),
			Created = table.Column<DateTimeOffset>(),
			Modified = table.Column<DateTimeOffset>(),
			CreatedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ModifiedBy = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			IsActivated = table.Column<bool>(),
			RequestId = table.Column<Guid>(),
			MeasurementId = table.Column<Guid>(null, null, null, rowVersion: false, null, nullable: true),
			Name = table.Column<string>(),
			Value = table.Column<string>(),
			Unit = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Upper = table.Column<double>(null, null, null, rowVersion: false, null, nullable: true),
			Lower = table.Column<double>(null, null, null, rowVersion: false, null, nullable: true),
			Result = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ResultOrigin = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Judge = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			MachineName = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			StaffName = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			History = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Sample = table.Column<int>()
		}, null, table =>
		{
			table.PrimaryKey("PK_RequestResults", x => x.Id);
			table.ForeignKey("FK_RequestResults_Measurements_MeasurementId", x => x.MeasurementId, "Measurements", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			table.ForeignKey("FK_RequestResults_Requests_RequestId", x => x.RequestId, "Requests", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateIndex("IX_AuthUsers_DepartmentId", "AuthUsers", "DepartmentId");
		migrationBuilder.CreateIndex("IX_Comments_RequestId", "Comments", "RequestId");
		migrationBuilder.CreateIndex("IX_Machines_FactoryId", "Machines", "FactoryId");
		migrationBuilder.CreateIndex("IX_Machines_MachineTypeId", "Machines", "MachineTypeId");
		migrationBuilder.CreateIndex("IX_Measurements_ImportantId", "Measurements", "ImportantId");
		migrationBuilder.CreateIndex("IX_Measurements_MachineTypeId", "Measurements", "MachineTypeId");
		migrationBuilder.CreateIndex("IX_Measurements_ProductId", "Measurements", "ProductId");
		migrationBuilder.CreateIndex("IX_Measurements_TemplateId", "Measurements", "TemplateId");
		migrationBuilder.CreateIndex("IX_Measurements_UnitId", "Measurements", "UnitId");
		migrationBuilder.CreateIndex("IX_MetadataTypes_ParentId", "MetadataTypes", "ParentId");
		migrationBuilder.CreateIndex("IX_MetadataValues_ParentId", "MetadataValues", "ParentId");
		migrationBuilder.CreateIndex("IX_MetadataValues_TypeId", "MetadataValues", "TypeId");
		migrationBuilder.CreateIndex("IX_PlanDetails_MeasurementId", "PlanDetails", "MeasurementId");
		migrationBuilder.CreateIndex("IX_PlanDetails_PlanId", "PlanDetails", "PlanId");
		migrationBuilder.CreateIndex("IX_Plans_ProductId", "Plans", "ProductId");
		migrationBuilder.CreateIndex("IX_Plans_TemplateId", "Plans", "TemplateId");
		migrationBuilder.CreateIndex("IX_Plans_TypeId", "Plans", "TypeId");
		migrationBuilder.CreateIndex("IX_Products_TemplateId", "Products", "TemplateId");
		migrationBuilder.CreateIndex("IX_RequestResults_MeasurementId", "RequestResults", "MeasurementId");
		migrationBuilder.CreateIndex("IX_RequestResults_RequestId", "RequestResults", "RequestId");
		migrationBuilder.CreateIndex("IX_Requests_LineId", "Requests", "LineId");
		migrationBuilder.CreateIndex("IX_Requests_MachineMoldId", "Requests", "MachineMoldId");
		migrationBuilder.CreateIndex("IX_Requests_MoldId", "Requests", "MoldId");
		migrationBuilder.CreateIndex("IX_Requests_ProductId", "Requests", "ProductId");
		migrationBuilder.CreateIndex("IX_Requests_TypeId", "Requests", "TypeId");
		migrationBuilder.CreateIndex("IX_Tools_MachineId", "Tools", "MachineId");
		migrationBuilder.CreateIndex("IX_Tools_TabletId", "Tools", "TabletId");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("AuthUsers");
		migrationBuilder.DropTable("Comments");
		migrationBuilder.DropTable("PlanDetails");
		migrationBuilder.DropTable("RequestResults");
		migrationBuilder.DropTable("Tools");
		migrationBuilder.DropTable("Plans");
		migrationBuilder.DropTable("Measurements");
		migrationBuilder.DropTable("Requests");
		migrationBuilder.DropTable("Machines");
		migrationBuilder.DropTable("Products");
		migrationBuilder.DropTable("MetadataValues");
		migrationBuilder.DropTable("Templates");
		migrationBuilder.DropTable("MetadataTypes");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.1.3").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("QA5SWebCore.Models.AuthUser", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Address").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset?>("BirthDate").HasColumnType("datetimeoffset");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("DepartmentId").HasColumnType("uniqueidentifier");
			b.Property<string>("Email").HasColumnType("nvarchar(max)");
			b.Property<string>("FullName").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Gender").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ImageUrl").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<string>("LoginAt").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Password").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
			b.Property<string>("Position").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("RefreshToken").HasColumnType("nvarchar(max)");
			b.Property<int>("Role").ValueGeneratedOnAdd().HasColumnType("int")
				.HasDefaultValue(3);
			b.Property<DateTimeOffset?>("TimeLogin").HasColumnType("datetimeoffset");
			b.Property<string>("Username").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("DepartmentId");
			b.ToTable("AuthUsers");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Comment", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Content").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid>("RequestId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("RequestId");
			b.ToTable("Comments");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Machine", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("FactoryId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<Guid>("MachineTypeId").HasColumnType("uniqueidentifier");
			b.Property<string>("Model").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Serial").HasColumnType("nvarchar(max)");
			b.Property<string>("Status").HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("FactoryId");
			b.HasIndex("MachineTypeId");
			b.ToTable("Machines");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Measurement", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Formula").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ImportantId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<double?>("Lower").HasColumnType("float");
			b.Property<Guid>("MachineTypeId").HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid>("ProductId").HasColumnType("uniqueidentifier");
			b.Property<int>("Sort").HasColumnType("int");
			b.Property<Guid?>("TemplateId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("UnitId").HasColumnType("uniqueidentifier");
			b.Property<double?>("Upper").HasColumnType("float");
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("ImportantId");
			b.HasIndex("MachineTypeId");
			b.HasIndex("ProductId");
			b.HasIndex("TemplateId");
			b.HasIndex("UnitId");
			b.ToTable("Measurements");
		});
		modelBuilder.Entity("QA5SWebCore.Models.MetadataType", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ParentId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("ParentId");
			b.ToTable("MetadataTypes");
		});
		modelBuilder.Entity("QA5SWebCore.Models.MetadataValue", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ParentId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("TypeId").HasColumnType("uniqueidentifier");
			b.Property<string>("Value").HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("ParentId");
			b.HasIndex("TypeId");
			b.ToTable("MetadataValues");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Plan", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid>("ProductId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("TemplateId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("TypeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("ProductId");
			b.HasIndex("TemplateId");
			b.HasIndex("TypeId");
			b.ToTable("Plans");
		});
		modelBuilder.Entity("QA5SWebCore.Models.PlanDetail", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<Guid?>("MeasurementId").HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid>("PlanId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("MeasurementId");
			b.HasIndex("PlanId");
			b.ToTable("PlanDetails");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Product", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<string>("ImageUrl").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<int>("Sample").HasColumnType("int");
			b.Property<int>("SampleMax").HasColumnType("int");
			b.Property<Guid?>("TemplateId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("TemplateId");
			b.ToTable("Products");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Request", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset?>("Approved").HasColumnType("datetimeoffset");
			b.Property<string>("ApprovedBy").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset?>("Checked").HasColumnType("datetimeoffset");
			b.Property<string>("CheckedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset?>("Completed").HasColumnType("datetimeoffset");
			b.Property<string>("CompletedBy").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Date").HasColumnType("datetimeoffset");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<string>("Judgement").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("LineId").HasColumnType("uniqueidentifier");
			b.Property<string>("Lot").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("MachineMoldId").HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("MoldId").HasColumnType("uniqueidentifier");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("PoNumber").HasColumnType("nvarchar(max)");
			b.Property<string>("PoQuantity").HasColumnType("nvarchar(max)");
			b.Property<Guid>("ProductId").HasColumnType("uniqueidentifier");
			b.Property<int>("Sample").HasColumnType("int");
			b.Property<bool>("Special").HasColumnType("bit");
			b.Property<string>("Status").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Supplier").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("TypeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("LineId");
			b.HasIndex("MachineMoldId");
			b.HasIndex("MoldId");
			b.HasIndex("ProductId");
			b.HasIndex("TypeId");
			b.ToTable("Requests");
		});
		modelBuilder.Entity("QA5SWebCore.Models.RequestResult", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("History").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<string>("Judge").HasColumnType("nvarchar(max)");
			b.Property<double?>("Lower").HasColumnType("float");
			b.Property<string>("MachineName").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("MeasurementId").HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid>("RequestId").HasColumnType("uniqueidentifier");
			b.Property<string>("Result").HasColumnType("nvarchar(max)");
			b.Property<string>("ResultOrigin").HasColumnType("nvarchar(max)");
			b.Property<int>("Sample").HasColumnType("int");
			b.Property<string>("StaffName").HasColumnType("nvarchar(max)");
			b.Property<string>("Unit").HasColumnType("nvarchar(max)");
			b.Property<double?>("Upper").HasColumnType("float");
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("MeasurementId");
			b.HasIndex("RequestId");
			b.ToTable("RequestResults");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Template", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Code").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<int>("Limit").HasColumnType("int");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("TemplateData").HasColumnType("nvarchar(max)");
			b.Property<string>("TemplateUrl").HasColumnType("nvarchar(max)");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("Templates");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Tool", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Created").HasColumnType("datetimeoffset");
			b.Property<string>("CreatedBy").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActivated").HasColumnType("bit");
			b.Property<Guid?>("MachineId").HasColumnType("uniqueidentifier");
			b.Property<DateTimeOffset>("Modified").HasColumnType("datetimeoffset");
			b.Property<string>("ModifiedBy").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("TabletId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("MachineId");
			b.HasIndex("TabletId");
			b.ToTable("Tools");
		});
		modelBuilder.Entity("QA5SWebCore.Models.AuthUser", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Department").WithMany().HasForeignKey("DepartmentId");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Comment", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Request", "Request").WithMany().HasForeignKey("RequestId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.Machine", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Factory").WithMany().HasForeignKey("FactoryId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "MachineType").WithMany().HasForeignKey("MachineTypeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.Measurement", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Important").WithMany().HasForeignKey("ImportantId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "MachineType").WithMany().HasForeignKey("MachineTypeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("QA5SWebCore.Models.Product", "Product").WithMany("Measurements").HasForeignKey("ProductId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("QA5SWebCore.Models.Template", "Template").WithMany().HasForeignKey("TemplateId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Unit").WithMany().HasForeignKey("UnitId");
		});
		modelBuilder.Entity("QA5SWebCore.Models.MetadataType", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataType", "Parent").WithMany().HasForeignKey("ParentId");
		});
		modelBuilder.Entity("QA5SWebCore.Models.MetadataValue", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Parent").WithMany().HasForeignKey("ParentId");
			b.HasOne("QA5SWebCore.Models.MetadataType", "Type").WithMany().HasForeignKey("TypeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.Plan", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Product", "Product").WithMany().HasForeignKey("ProductId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("QA5SWebCore.Models.Template", "Template").WithMany().HasForeignKey("TemplateId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Type").WithMany().HasForeignKey("TypeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.PlanDetail", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Measurement", "Measurement").WithMany().HasForeignKey("MeasurementId");
			b.HasOne("QA5SWebCore.Models.Plan", "Plan").WithMany("PlanDetails").HasForeignKey("PlanId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.Product", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Template", "Template").WithMany().HasForeignKey("TemplateId");
		});
		modelBuilder.Entity("QA5SWebCore.Models.Request", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Line").WithMany().HasForeignKey("LineId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "MachineMold").WithMany().HasForeignKey("MachineMoldId");
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Mold").WithMany().HasForeignKey("MoldId");
			b.HasOne("QA5SWebCore.Models.Product", "Product").WithMany().HasForeignKey("ProductId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("QA5SWebCore.Models.MetadataValue", "Type").WithMany().HasForeignKey("TypeId");
		});
		modelBuilder.Entity("QA5SWebCore.Models.RequestResult", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Measurement", "Measurement").WithMany().HasForeignKey("MeasurementId");
			b.HasOne("QA5SWebCore.Models.Request", "Request").WithMany().HasForeignKey("RequestId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("QA5SWebCore.Models.Tool", delegate(EntityTypeBuilder b)
		{
			b.HasOne("QA5SWebCore.Models.Machine", "Machine").WithMany().HasForeignKey("MachineId");
			b.HasOne("QA5SWebCore.Models.Machine", "Tablet").WithMany().HasForeignKey("TabletId");
		});
	}
}
