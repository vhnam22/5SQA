using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QA5SWebCore.Data;

namespace QA5SWebCore.Migrations;

[DbContext(typeof(RepositoryContext))]
internal class RepositoryContextModelSnapshot : ModelSnapshot
{
	protected override void BuildModel(ModelBuilder modelBuilder)
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
