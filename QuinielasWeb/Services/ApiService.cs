using Newtonsoft.Json;
using System.Text;

namespace QuinielasWeb.Services;

public class ApiService
{
    protected readonly HttpClient _client = new();
    protected readonly HttpClientHandler _clientHandler = new();
    protected readonly string baseUrl = "https://localhost:7242/";

    protected async Task<T> Get<T>(string path)
    {
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var response = await _client.GetAsync(path);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 300)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }

    protected async Task<T> Post<T>(string path, object? data)
    {
        var json_ = JsonConvert.SerializeObject(data);
        var content = new StringContent(json_, Encoding.UTF8, "application/json");
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var response = await _client.PostAsync(path, content);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 300)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }

    protected async Task<T> Put<T>(string path, object? data)
    {
        var json_ = JsonConvert.SerializeObject(data);
        var content = new StringContent(json_, Encoding.UTF8, "application/json");
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var response = await _client.PutAsync(path, content);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 300)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }

    protected async Task<T> Delete<T>(string path)
    {
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var response = await _client.DeleteAsync(path);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 300)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }
}
