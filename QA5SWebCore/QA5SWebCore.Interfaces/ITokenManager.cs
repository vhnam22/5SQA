using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QA5SWebCore.Interfaces;

public interface ITokenManager
{
	Task<bool> IsCurrentActiveToken();

	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

	Guid GetUserIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);

	bool IsExpired();
}
