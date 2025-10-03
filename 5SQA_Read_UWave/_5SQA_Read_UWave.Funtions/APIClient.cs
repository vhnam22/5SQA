using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _5SQA_Read_UWave.Funtions;

public class APIClient
{
	private string _baseUrl;

	private HttpClient _httpClient;

	private Lazy<JsonSerializerSettings> _settings;

	public APIClient(string baseUrl)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		_baseUrl = baseUrl;
		_httpClient = new HttpClient();
		_settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
	}

	public void CloseAPI()
	{
		if (_httpClient != null)
		{
			((HttpMessageInvoker)_httpClient).Dispose();
		}
	}

	private JsonSerializerSettings CreateSerializerSettings()
	{
		return new JsonSerializerSettings();
	}

	public Task SaveAsync(object body, string uriName)
	{
		return SaveAsync(body, CancellationToken.None, uriName);
	}

	public async Task SaveAsync(object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		HttpRequestMessage request_ = new HttpRequestMessage();
		try
		{
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			((HttpContent)content_).Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = (HttpContent)(object)content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (response_ != null)
			{
				response_.Dispose();
			}
		}
		finally
		{
			((IDisposable)request_)?.Dispose();
		}
	}
}
