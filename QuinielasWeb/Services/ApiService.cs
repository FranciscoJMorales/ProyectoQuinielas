using Newtonsoft.Json;
using System.Text;

namespace QuinielasWeb.Services;

public class ApiService
{
    protected readonly string baseUrl;
    private readonly IHttpContextAccessor _accessor;

    public ApiService(IHttpContextAccessor accessor)
    {
        baseUrl = "https://localhost/QuinielasApi/";
        _accessor = accessor;
    }

    protected async Task<T> Get<T>(string path)
    {
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var _httpContext = _accessor.HttpContext;
        var token = _httpContext!.Session.GetString("token");
        client.DefaultRequestHeaders.Authorization = null;
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync(path);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 500)
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
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        var json_ = JsonConvert.SerializeObject(data);
        var content = new StringContent(json_, Encoding.UTF8, "application/json");
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var _httpContext = _accessor.HttpContext;
        var token = _httpContext!.Session.GetString("token");
        client.DefaultRequestHeaders.Authorization = null;
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PostAsync(path, content);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 500)
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
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        var json_ = JsonConvert.SerializeObject(data);
        var content = new StringContent(json_, Encoding.UTF8, "application/json");
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var _httpContext = _accessor.HttpContext;
        var token = _httpContext!.Session.GetString("token");
        client.DefaultRequestHeaders.Authorization = null;
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PutAsync(path, content);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 500)
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
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var _httpContext = _accessor.HttpContext;
        var token = _httpContext!.Session.GetString("token");
        client.DefaultRequestHeaders.Authorization = null;
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.DeleteAsync(path);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 500)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }

    protected async Task<T> PostNoAuth<T>(string path, object? data)
    {
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        var json_ = JsonConvert.SerializeObject(data);
        var content = new StringContent(json_, Encoding.UTF8, "application/json");
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        var response = await client.PostAsync(path, content);
        int statusCode = (int)response.StatusCode;
        if (statusCode >= 200 && statusCode < 500)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
        }
        else
        {
            throw new Exception(response.StatusCode.ToString());
        }
    }

}
