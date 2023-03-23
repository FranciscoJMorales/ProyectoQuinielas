namespace QuinielasWeb.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int StatusCode { get; set; }
        public string Status { get; set; } = String.Empty;
        public bool IsLoggedIn { get; set; }
        public string? Message { get; set; }
    }
}