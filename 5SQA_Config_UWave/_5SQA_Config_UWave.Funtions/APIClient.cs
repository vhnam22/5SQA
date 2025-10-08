using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _5SQA_Config_UWave.Dtos;
using Newtonsoft.Json;

namespace _5SQA_Config_UWave.Funtions;

public class APIClient
{
	protected struct ObjectResponseResult<T>
	{
		public T Object { get; }

		public string Text { get; }

		public ObjectResponseResult(T responseObject, string responseText)
		{
			Object = responseObject;
			Text = responseText;
		}
	}

	private string _baseUrl;

	private HttpClient _httpClient;

	private Lazy<JsonSerializerSettings> _settings;

	protected JsonSerializerSettings JsonSerializerSettings => _settings.Value;

	public bool ReadResponseAsString { get; set; }

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

	public Task<ResponseDto> GetsAsync(QueryArgs body, string uriName)
	{
		return GetsAsync(body, CancellationToken.None, uriName);
	}

	public async Task<ResponseDto> GetsAsync(QueryArgs body, CancellationToken cancellationToken, string uriName)
	{
		StringBuilder urlBuilder_ = new StringBuilder();
		urlBuilder_.Append((_baseUrl != null) ? _baseUrl.TrimEnd('/') : "").Append(uriName);
		HttpClient client_ = _httpClient;
		try
		{
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
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = ((IEnumerable<KeyValuePair<string, IEnumerable<string>>>)response_.Headers).ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item_ in (HttpHeaders)response_.Content.Headers)
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
					if (response_ != null)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
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
