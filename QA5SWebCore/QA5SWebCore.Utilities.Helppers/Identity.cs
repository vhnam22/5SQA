using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using QA5SWebCore.Utilities.Enums;

namespace QA5SWebCore.Utilities.Helppers;

public static class Identity
{
	public static Guid GetUserId(ClaimsPrincipal identity)
	{
		if (identity.Claims != null)
		{
			var source = identity.Claims.Select((Claim x) => new { x.Value, x.Type });
			var anon = source.LastOrDefault(x => x.Type.Equals(new IdentityOptions().ClaimsIdentity.UserIdClaimType));
			if (anon != null)
			{
				return new Guid(anon.Value);
			}
		}
		return Guid.Empty;
	}

	public static RoleWeb GetUserRole(ClaimsPrincipal identity)
	{
		if (identity.Claims != null)
		{
			var source = identity.Claims.Select((Claim x) => new { x.Value, x.Type });
			var anon = source.LastOrDefault(x => x.Type.Equals(new IdentityOptions().ClaimsIdentity.RoleClaimType));
			if (anon != null)
			{
				switch (anon.Value)
				{
				case "Member":
					return RoleWeb.Member;
				case "Administrator":
					return RoleWeb.Administrator;
				case "SuperAdministrator":
					return RoleWeb.SuperAdministrator;
				}
			}
		}
		return RoleWeb.None;
	}

	public static string GetUserFullName(ClaimsPrincipal identity)
	{
		if (identity.Claims != null)
		{
			var source = identity.Claims.Select((Claim x) => new { x.Value, x.Type });
			var anon = source.LastOrDefault(x => x.Type.Equals(new IdentityOptions().ClaimsIdentity.UserNameClaimType));
			if (anon != null)
			{
				return anon.Value.ToString();
			}
		}
		return string.Empty;
	}
}
