using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using Newtonsoft.Json;

namespace _5S_QA_DL.APIContext;

public class APIClient
{
	protected readonly struct ObjectResponseResult<T>
	{
		public T Object { get; }

		public string Text { get; }

		public ObjectResponseResult(T responseObject, string responseText)
		{
			Object = responseObject;
			Text = responseText;
		}
	}

	private readonly string _baseUrl;

	private string _token;

	private readonly HttpClient _httpClient;

	private readonly Lazy<JsonSerializerSettings> _settings;

	protected JsonSerializerSettings JsonSerializerSettings => _settings.Value;

	public string Token
	{
		get
		{
			return _token;
		}
		set
		{
			_token = value;
		}
	}

	public bool ReadResponseAsString { get; set; }

    public string IME { get; set; }

    public APIClient()
	{
		_baseUrl = APIUrl.APIHost;
		_httpClient = new HttpClient();
		_settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
	}

	public void CloseAPI()
	{
		_httpClient?.Dispose();
	}

	private JsonSerializerSettings CreateSerializerSettings()
	{
		return new JsonSerializerSettings();
	}

	public Task<ResponseDto> LoginAsync(LoginDto body)
	{
		return LoginAsync(body, CancellationToken.None);
	}

	public async Task<ResponseDto> LoginAsync(LoginDto body, CancellationToken cancellationToken)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append("/api/AuthUser/Login");
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Headers.Add("TYPE", "System");
			request_.Headers.Add("IME", "");
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> LogoutAsync(Guid id)
	{
		return LogoutAsync(id, CancellationToken.None);
	}

	public async Task<ResponseDto> LogoutAsync(Guid id, CancellationToken cancellationToken)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append("/api/AuthUser/Logout/{id}");
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto body)
	{
		return ChangePasswordAsync(body, CancellationToken.None);
	}

	public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto body, CancellationToken cancellationToken)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append("/api/AuthUser/ChangeMyPassword");
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetsAsync(string uriName)
	{
		return GetsAsync(CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetsAsync(object body, string uriName)
	{
		return GetsAsync(body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetsAsync(Guid id, string uriName)
	{
		return GetsAsync(id, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

    public async Task<ResponseDto> GetsAsync(Guid id, QueryArgs body, CancellationToken cancellationToken, string uriName)
    {
        StringBuilder urlBuilder_ = new StringBuilder();
        urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
        urlBuilder_.Replace("{id}", id.ToString());
        HttpClient client_ = _httpClient;
        try
        {
            using HttpRequestMessage request_ = new HttpRequestMessage();
            StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
            content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
            request_.Content = content_;
            request_.Method = new HttpMethod("POST");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            try
            {
                Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
                if (response_.Content != null && response_.Content.Headers != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
                    {
                        headers_[item_.Key] = item_.Value;
                    }
                }
                string status_ = ((int)response_.StatusCode).ToString();
                if (status_.Equals("200"))
                {
                    return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
                }
                if (!status_.Equals("200") && !status_.Equals("204"))
                {
                    string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                    string responseData_ = text;
                    throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                }
                return null;
            }
            finally
            {
                response_?.Dispose();
            }
        }
        finally
        {
            
        }
    }

    public Task<ResponseDto> GetsAsync(Guid id, QueryArgs body, string uriName)
    {
        return GetsAsync(id, body, CancellationToken.None, uriName);
    }

    public Task<ResponseDto> GetsAsync(Guid productid, Guid id, string uriName)
	{
		return GetsAsync(productid, id, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(Guid productid, Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{productid}", productid.ToString()).Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetsAsync(Guid id, int sample, IEnumerable<string> body, string uriName)
	{
		return GetsAsync(id, sample, body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(Guid id, int sample, IEnumerable<string> body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString()).Replace("{sample}", sample.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetsAsync(Guid id, int sample, string uriName)
	{
		return GetsAsync(id, sample, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(Guid id, int sample, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString()).Replace("{sample}", sample.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> GetResultsAsync(GetResultDto body, string uriName)
	{
		return GetResultsAsync(body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetResultsAsync(GetResultDto body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_ == "200")
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (status_ != "200" && status_ != "204")
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> DeleteAsync(Guid id, string uriName)
	{
		return DeleteAsync(id, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> DeleteAsync(Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("DELETE");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> MoveAsync(Guid idfrom, Guid idto, string uriName)
	{
		return MoveAsync(idfrom, idto, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> MoveAsync(Guid idfrom, Guid idto, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{idfrom}", idfrom.ToString()).Replace("{idto}", idto.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> SaveAsync(object body, string uriName)
	{
		return SaveAsync(body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> SaveAsync(object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> SaveAsync(Guid id, object body, string uriName)
	{
		return SaveAsync(id, body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> SaveAsync(Guid id, object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> ImportAsync(Guid id, FileParameter file, string uriName)
	{
		return ImportAsync(id, file, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> ImportAsync(Guid id, FileParameter file, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			string boundary_ = Guid.NewGuid().ToString();
			MultipartFormDataContent content_ = new MultipartFormDataContent(boundary_);
			content_.Headers.Remove("Content-Type");
			content_.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary_);
			if (file != null)
			{
				StreamContent content_file_ = new StreamContent(file.Data);
				if (!string.IsNullOrEmpty(file.ContentType))
				{
					content_file_.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
				}
				content_.Add(content_file_, "file", file.FileName ?? "file");
			}
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ExportExcelDto> DownloadAsync(Guid id, string uriName)
	{
		return DownloadAsync(id, CancellationToken.None, uriName);
	}

	public async Task<ExportExcelDto> DownloadAsync(Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					ExportExcelDto exportExcelDto = new ExportExcelDto
					{
						FileName = response_.Content.Headers.ContentDisposition.FileName
					};
					ExportExcelDto exportExcelDto2 = exportExcelDto;
					exportExcelDto2.Value = await response_.Content.ReadAsByteArrayAsync();
					return exportExcelDto;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ExportExcelDto> ExportAsync(Guid id, string uriName)
	{
		return ExportAsync(id, CancellationToken.None, uriName);
	}

	public async Task<ExportExcelDto> ExportAsync(Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					ExportExcelDto exportExcelDto = new ExportExcelDto
					{
						FileName = response_.Content.Headers.ContentDisposition.FileName
					};
					ExportExcelDto exportExcelDto2 = exportExcelDto;
					exportExcelDto2.Value = await response_.Content.ReadAsByteArrayAsync();
					return exportExcelDto;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ExportExcelDto> ExportAsync(object body, string uriName)
	{
		return ExportAsync(body, CancellationToken.None, uriName);
	}

	public async Task<ExportExcelDto> ExportAsync(object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					ExportExcelDto exportExcelDto = new ExportExcelDto
					{
						FileName = response_.Content.Headers.ContentDisposition.FileName
					};
					ExportExcelDto exportExcelDto2 = exportExcelDto;
					exportExcelDto2.Value = await response_.Content.ReadAsByteArrayAsync();
					return exportExcelDto;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ExportExcelDto> ExportAsync(Guid id, object body, string uriName)
	{
		return ExportAsync(id, body, CancellationToken.None, uriName);
	}

	public async Task<ExportExcelDto> ExportAsync(Guid id, object body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			StringContent content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
			content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
			request_.Content = content_;
			request_.Method = new HttpMethod("POST");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					ExportExcelDto exportExcelDto = new ExportExcelDto
					{
						FileName = response_.Content.Headers.ContentDisposition.FileName
					};
					ExportExcelDto exportExcelDto2 = exportExcelDto;
					exportExcelDto2.Value = await response_.Content.ReadAsByteArrayAsync();
					return exportExcelDto;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	public Task<ResponseDto> CopyAsync(Guid id, string uriName)
	{
		return CopyAsync(id, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> CopyAsync(Guid id, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		urlBuilder_.Replace("{id}", id.ToString());
		HttpClient client_ = _httpClient;
		try
		{
			using HttpRequestMessage request_ = new HttpRequestMessage();
			request_.Method = new HttpMethod("GET");
			request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
			request_.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			string url_ = urlBuilder_.ToString();
			request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
				if (response_.Content != null && response_.Content.Headers != null)
				{
					foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
					{
						headers_[item_.Key] = item_.Value;
					}
				}
				string status_ = ((int)response_.StatusCode).ToString();
				if (status_.Equals("200"))
				{
					return (await ReadObjectResponseAsync<ResponseDto>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false)).Object;
				}
				if (!status_.Equals("200") && !status_.Equals("204"))
				{
					string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
					string responseData_ = text;
					throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
				}
				return null;
			}
			finally
			{
				response_?.Dispose();
			}
		}
		finally
		{
			
		}
	}

	protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
	{
		if (response == null || response.Content == null)
		{
			return new ObjectResponseResult<T>(default(T), string.Empty);
		}
		if (ReadResponseAsString)
		{
			string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				T typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
				return new ObjectResponseResult<T>(typedBody, responseText);
			}
			catch (JsonException ex)
			{
				JsonException exception = ex;
				string message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
				throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
			}
		}
		try
		{
			using Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			using StreamReader streamReader = new StreamReader(responseStream);
			using JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
			JsonSerializer serializer = JsonSerializer.Create(JsonSerializerSettings);
			T typedBody2 = serializer.Deserialize<T>(jsonTextReader);
			return new ObjectResponseResult<T>(typedBody2, string.Empty);
		}
		catch (JsonException innerException)
		{
			string message2 = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
			throw new ApiException(message2, (int)response.StatusCode, string.Empty, headers, innerException);
		}
	}
}
