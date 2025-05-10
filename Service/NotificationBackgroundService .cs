using Asan_Campus.Model;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Asan_Campus.Service
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<NotificationBackgroundService> _logger;
        private const int IntervalMinutes = 2; // Run every 2 minutes

        public NotificationBackgroundService(
            IServiceProvider services,
            ILogger<NotificationBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Service running");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Starting notification check at: {DateTime.Now}");
                    await SendDailyNotifications();
                    _logger.LogInformation($"Notification check completed. Waiting {IntervalMinutes} minutes...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during notification processing");
                }
                finally
                {
                    // Wait for 2 minutes before next execution
                    await Task.Delay(TimeSpan.FromMinutes(IntervalMinutes), stoppingToken);
                }
            }
        }

        private async Task SendDailyNotifications()
        
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var today = DateTime.Today;
            var notifications = dbContext.Notifications
                .AsEnumerable()
                .Where(n => {
                    try
                    {
                        var date = DateTime.Parse(n.date);
                        return date.Date == today && !(n.IsSent ?? false);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();

            foreach (var notification in notifications)
            {
                var devices = await dbContext.UserDevices
                    .Include(d => d.Student)
                    .Where(d => d.Student.DepartmentId == notification.DepartmentID &&
                               d.Student.Semester == notification.SemesterID)
                    .ToListAsync();

                foreach (var device in devices)
                {
                    await SendPushNotification(device.ExpoPushToken, notification);
                }

                notification.IsSent = true;
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SendPushNotification(string token, Notification notification)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://exp.host/--/api/v2/push/send");

            var payload = new
            {
                to = token,
                title = notification.Title,
                body = notification.Message,
                sound = "default",
                channelId = "default",
                priority = "high"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(new[] { payload }),
                Encoding.UTF8,
                "application/json");

            await httpClient.PostAsync("", content);
        }
    }
}