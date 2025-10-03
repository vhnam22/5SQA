using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Enums;
using QA5SWebCore.Utilities.Helppers;

namespace QA5SWebCore.Data;

public class DBInitializer
{
	public static void Initialize(RepositoryContext context, IConfiguration configuration)
	{
		context.Database.EnsureCreated();
		bool flag = true;
		if (!context.AuthUsers.Any())
		{
			context.AuthUsers.Add(new AuthUser
			{
				Id = default(Guid),
				FullName = "Admin",
				BirthDate = new DateTime(2020, 1, 1),
				Email = "aatechnology.co@gmail.com",
				Username = "Admin".ToLower(),
				PhoneNumber = "0348 156 154",
				Gender = "Male",
				Address = "233/57/7 Đường TTH 07, khu phố 3, Phường Tân Thới Hiệp, Quận 12, Thành phố Hồ Chí Minh",
				Password = Encryptor.GenerateSaltedSHA1("1234567@A"),
				Position = "Manager",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now,
				Role = RoleWeb.SuperAdministrator
			});
			flag = false;
		}
		if (!context.Emails.Any())
		{
			context.Emails.Add(new Email
			{
				Id = default(Guid),
				Name = "",
				Password = "",
				Header = "<p>Dear Mr/Ms.</p><p>This is an automated 5SQA notification email, please do not reply to this email.</p>",
				Footer = "<p>--Copyright @2015--</p><p>5SQA</p>",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			flag = false;
		}
		if (!context.MetadataTypes.Any())
		{
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("55630EBA-6A11-4001-B161-9AE77ACCA43D"),
				Code = "DEPARTMENT",
				Name = "Phòng ban",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("438D7052-25F3-4342-ED0C-08D7E9C5C77D"),
				Code = "MACHINETYPE",
				Name = "Loại máy",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476"),
				Code = "FACTORY",
				Name = "Nhà xưởng",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("6042BF53-9411-47D4-9BD6-F8AB7BABB663"),
				Code = "IMPORTANT",
				Name = "Độ quan trọng",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D"),
				Code = "UNIT",
				Name = "Đơn vị",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("11C5FD56-AD45-4457-8DC9-6C8D9F6673A1"),
				Code = "STAGE",
				Name = "Công đoạn",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("AC5FA813-C9EE-4805-A850-30A5EA5AB0A1"),
				Code = "TYPE",
				Name = "Kiểu",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("AC5FA814-C9EE-4807-A851-30A5EA5AB0A2"),
				Code = "LINE",
				Name = "Chuyền",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("3FCB0099-A290-46A6-A2C4-1934C6328B9D"),
				Code = "CONFIG",
				Name = "Cấu hình",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataTypes.Add(new MetadataType
			{
				Id = new Guid("AC5FA815-C9EE-4807-A852-30A5EA5AB0A3"),
				Code = "TYPECAL",
				Name = "Kiểu hiệu chuẩn",
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			flag = false;
		}
		if (!context.MetadataValues.Any())
		{
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "CFG-1",
				Name = "DirFile",
				TypeId = Guid.Parse("3FCB0099-A290-46A6-A2C4-1934C6328B9D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "IMP-1",
				Name = "*",
				TypeId = Guid.Parse("6042BF53-9411-47D4-9BD6-F8AB7BABB663"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "MTYPE-1",
				Name = "Tablet",
				TypeId = Guid.Parse("438D7052-25F3-4342-ED0C-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "MTYPE-2",
				Name = "Manual Input",
				TypeId = Guid.Parse("438D7052-25F3-4342-ED0C-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "UNIT-1",
				Name = "µm",
				TypeId = Guid.Parse("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "UNIT-2",
				Name = "m",
				TypeId = Guid.Parse("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "UNIT-3",
				Name = "°",
				TypeId = Guid.Parse("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.MetadataValues.Add(new MetadataValue
			{
				Id = default(Guid),
				Code = "UNIT-4",
				Name = "mm",
				TypeId = Guid.Parse("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D"),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			flag = false;
		}
		if (!context.Constants.Any())
		{
			context.Constants.Add(new Constant
			{
				n = 2,
				A2 = 1.88,
				A3 = 2.659,
				d2 = 1.128,
				c4 = 0.7979,
				B3 = 0.0,
				B4 = 3.267,
				D3 = 0.0,
				D4 = 3.267,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 3,
				A2 = 1.023,
				A3 = 1.954,
				d2 = 1.693,
				c4 = 0.8862,
				B3 = 0.0,
				B4 = 2.568,
				D3 = 0.0,
				D4 = 2.575,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 4,
				A2 = 0.729,
				A3 = 1.628,
				d2 = 2.059,
				c4 = 0.9213,
				B3 = 0.0,
				B4 = 2.266,
				D3 = 0.0,
				D4 = 2.282,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 5,
				A2 = 0.577,
				A3 = 1.427,
				d2 = 2.326,
				c4 = 0.94,
				B3 = 0.0,
				B4 = 2.089,
				D3 = 0.0,
				D4 = 2.115,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 6,
				A2 = 0.483,
				A3 = 1.287,
				d2 = 2.534,
				c4 = 0.9515,
				B3 = 0.03,
				B4 = 1.97,
				D3 = 0.0,
				D4 = 2.004,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 7,
				A2 = 0.419,
				A3 = 1.182,
				d2 = 2.704,
				c4 = 0.9594,
				B3 = 0.118,
				B4 = 1.882,
				D3 = 0.076,
				D4 = 1.924,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 8,
				A2 = 0.373,
				A3 = 1.099,
				d2 = 2.847,
				c4 = 0.965,
				B3 = 0.185,
				B4 = 0.185,
				D3 = 0.136,
				D4 = 1.864,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 9,
				A2 = 0.337,
				A3 = 1.032,
				d2 = 2.97,
				c4 = 0.9693,
				B3 = 0.239,
				B4 = 1.761,
				D3 = 0.184,
				D4 = 1.816,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 10,
				A2 = 0.308,
				A3 = 0.975,
				d2 = 3.078,
				c4 = 0.9727,
				B3 = 0.284,
				B4 = 1.716,
				D3 = 0.223,
				D4 = 1.777,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 11,
				A2 = 0.285,
				A3 = 0.927,
				d2 = 3.173,
				c4 = 0.9754,
				B3 = 0.321,
				B4 = 1.679,
				D3 = 0.256,
				D4 = 1.744,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 12,
				A2 = 0.266,
				A3 = 0.886,
				d2 = 3.258,
				c4 = 0.9776,
				B3 = 0.354,
				B4 = 1.646,
				D3 = 0.283,
				D4 = 1.717,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 13,
				A2 = 0.249,
				A3 = 0.85,
				d2 = 3.336,
				c4 = 0.9794,
				B3 = 0.382,
				B4 = 1.618,
				D3 = 0.307,
				D4 = 1.693,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 14,
				A2 = 0.235,
				A3 = 0.817,
				d2 = 3.407,
				c4 = 0.981,
				B3 = 0.406,
				B4 = 1.594,
				D3 = 0.328,
				D4 = 1.672,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 15,
				A2 = 0.223,
				A3 = 0.789,
				d2 = 3.472,
				c4 = 0.9823,
				B3 = 0.428,
				B4 = 1.572,
				D3 = 0.347,
				D4 = 1.653,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 16,
				A2 = 0.212,
				A3 = 0.763,
				d2 = 3.532,
				c4 = 0.9835,
				B3 = 0.448,
				B4 = 1.552,
				D3 = 0.363,
				D4 = 1.637,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 17,
				A2 = 0.203,
				A3 = 0.739,
				d2 = 3.588,
				c4 = 0.9845,
				B3 = 0.466,
				B4 = 1.534,
				D3 = 0.378,
				D4 = 1.622,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 18,
				A2 = 0.194,
				A3 = 0.718,
				d2 = 3.64,
				c4 = 0.9854,
				B3 = 0.482,
				B4 = 1.518,
				D3 = 0.391,
				D4 = 1.608,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 19,
				A2 = 0.187,
				A3 = 0.698,
				d2 = 3.689,
				c4 = 0.9862,
				B3 = 0.497,
				B4 = 1.503,
				D3 = 0.403,
				D4 = 1.597,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 20,
				A2 = 0.18,
				A3 = 0.68,
				d2 = 3.735,
				c4 = 0.0969,
				B3 = 0.51,
				B4 = 1.49,
				D3 = 0.415,
				D4 = 1.585,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 21,
				A2 = 0.173,
				A3 = 0.663,
				d2 = 3.778,
				c4 = 0.9876,
				B3 = 0.523,
				B4 = 1.477,
				D3 = 0.425,
				D4 = 1.575,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 22,
				A2 = 0.167,
				A3 = 0.647,
				d2 = 3.819,
				c4 = 0.9882,
				B3 = 0.534,
				B4 = 1.466,
				D3 = 0.434,
				D4 = 1.566,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 23,
				A2 = 0.162,
				A3 = 0.633,
				d2 = 3.858,
				c4 = 0.9887,
				B3 = 0.545,
				B4 = 1.455,
				D3 = 0.443,
				D4 = 1.557,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 24,
				A2 = 0.157,
				A3 = 0.619,
				d2 = 3.895,
				c4 = 0.9892,
				B3 = 0.555,
				B4 = 1.445,
				D3 = 0.451,
				D4 = 1.548,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			context.Constants.Add(new Constant
			{
				n = 25,
				A2 = 0.153,
				A3 = 0.606,
				d2 = 3.931,
				c4 = 0.9896,
				B3 = 0.565,
				B4 = 1.435,
				D3 = 0.459,
				D4 = 1.541,
				E2 = 0.0,
				Id = default(Guid),
				IsActivated = true,
				CreatedBy = "Admin",
				ModifiedBy = "Admin",
				Created = DateTimeOffset.Now,
				Modified = DateTimeOffset.Now
			});
			flag = false;
		}
		if (!flag)
		{
			context.SaveChanges();
		}
	}
}
