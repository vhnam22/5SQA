using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QA5SWebCore.Interfaces;

namespace QA5SWebCore.Middleware;

public class TokenManagerMiddleware : IMiddleware
{
	private readonly ITokenManager _tokenManager;

	public TokenManagerMiddleware(ITokenManager tokenManager)
	{
		_tokenManager = tokenManager;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (!_tokenManager.IsExpired())
		{
			context.Response.StatusCode = 401;
			context.Response.Headers.Add("Expired", "true");
		}
		else if (!(await _tokenManager.IsCurrentActiveToken()))
		{
			context.Response.StatusCode = 401;
			context.Response.Headers.Add("Another-Logged-In", "true");
		}
		else
		{
			await next(context);
		}
	}
}
