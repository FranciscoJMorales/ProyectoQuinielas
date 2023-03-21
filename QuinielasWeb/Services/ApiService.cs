namespace QuinielasWeb.Services;

public class ApiService
{
    protected readonly HttpClient _client = new();
    protected readonly HttpClientHandler _clientHandler = new();
    protected readonly string url = "https://localhost:7242/";
}
