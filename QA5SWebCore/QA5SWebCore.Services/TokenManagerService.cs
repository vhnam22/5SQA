using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using QA5SWebCore.Data.Interfaces;
using QA5SWebCore.Interfaces;
using QA5SWebCore.Models;
using QA5SWebCore.Utilities.Dtos;
using QA5SWebCore.Utilities.Helppers;

namespace QA5SWebCore.Services;

public class TokenManagerService : ITokenManager
{
	private string USERID_TYPE = "UserId";

	private readonly IHttpContextAccessor _httpContextAccessor;

	private readonly IUnitOfWork _uow;

	private readonly IConfiguration _configuration;

	public TokenManagerService(IHttpContextAccessor httpContextAccessor, IUnitOfWork uow, IConfiguration configuration)
	{
		_uow = uow;
		_httpContextAccessor = httpContextAccessor;
		_configuration = configuration;
	}

	public async Task<bool> IsCurrentActiveToken()
	{
		string token = GetCurrentAuthorization();
		if (!string.IsNullOrEmpty(token))
		{
			ClaimsPrincipal principalFromExpiredToken = GetPrincipalFromExpiredToken(token);
			Guid userIdFromClaimsPrincipal = GetUserIdFromClaimsPrincipal(principalFromExpiredToken);
			AuthUser authUser = await _uow.GetRepository<AuthUser>().FindByIdAsync(userIdFromClaimsPrincipal);
			if (authUser != null && !token.Equals(authUser.RefreshToken))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsExpired()
	{
		string cryptedString = _configuration["Key"];
		ConfigApp configApp = JsonSerializer.Deserialize<ConfigApp>(DesLogHelper.Decrypt(cryptedString, "GetBytes"));
		if (configApp.NumDay.HasValue && configApp.ActiveDate.AddDays(configApp.NumDay.Value) < DateTime.Now)
		{
			return false;
		}
		return true;
	}

	private string GetCurrentAuthorization()
	{
		StringValues stringValues = _httpContextAccessor.HttpContext.Request.Headers["authorization"];
		if (!(stringValues == StringValues.Empty))
		{
			return stringValues.Single().Split(" ").Last();
		}
		return string.Empty;
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		TokenValidationParameters validationParameters = new TokenValidationParameters
		{
			ValidateLifetime = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A0B5E65FDBBBC4FB18946D4A9CCDBAB4A")),
			RequireExpirationTime = true,
			ClockSkew = TimeSpan.Zero,
			ValidateIssuer = false,
			ValidateAudience = false
		};
		JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		SecurityToken validatedToken;
		ClaimsPrincipal result = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out validatedToken);
		if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals("HS256", StringComparison.InvariantCultureIgnoreCase))
		{
			throw new SecurityTokenException("Invalid token");
		}
		return result;
	}

	public Guid GetUserIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
	{
		Claim claim = claimsPrincipal.Claims.FirstOrDefault((Claim k) => k.Type.Equals(USERID_TYPE));
		Guid result = Guid.Empty;
		Guid.TryParse(claim?.Value.ToString(), out result);
		return result;
	}
}
