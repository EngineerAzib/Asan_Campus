// ExpoNotificationService.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ExpoNotificationService
{
    private readonly HttpClient _httpClient;

    public ExpoNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://exp.host/--/api/v2/push/");
    }

    public async Task SendPushNotification(string expoPushToken, string title, string message)
    {
        var notification = new
        {
            to = expoPushToken,
            title = title,
            body = message,
            sound = "default",
            channelId = "default",
            priority = "high",
            _displayInForeground = false
        };

        var content = new StringContent(
            JsonSerializer.Serialize(new[] { notification }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("send", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send notification: {error}");
        }
    }
}