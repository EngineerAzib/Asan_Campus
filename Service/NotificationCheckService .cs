//using Asan_Campus.Model;
//using Microsoft.EntityFrameworkCore;

//namespace Asan_Campus.Service
//{
//    // NotificationCheckService.cs
//    public class NotificationCheckService : BackgroundService
//    {
//        private readonly IServiceProvider _services;
//        private readonly ILogger<NotificationCheckService> _logger;

//        public NotificationCheckService(IServiceProvider services, ILogger<NotificationCheckService> logger)
//        {
//            _services = services;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                var now = DateTime.Now;
//                var nextRun = now.Date.AddDays(1); // Next day at midnight

//                if (now > now.Date.AddHours(12)) // If it's past noon, wait until next day
//                {
//                    nextRun = nextRun.AddHours(12); // Adjust to 12 AM
//                }

//                var delay = nextRun - now;
//                _logger.LogInformation($"Next notification check at: {nextRun}");

//                await Task.Delay(delay, stoppingToken);

//                if (!stoppingToken.IsCancellationRequested)
//                {
//                    await CheckAndSendNotifications();
//                }
//            }
//        }

//        private async Task CheckAndSendNotifications()
//        {
//            using var scope = _services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            var notificationService = scope.ServiceProvider.GetRequiredService<ExpoNotificationService>();

//            var today = DateTime.Today;

//            // Get all notifications scheduled for today that haven't been sent
//            var today = DateTime.Today;
//            var notifications = await dbContext.Notifications
//                .Where(n => EF.Functions.DateFromParts(
//                    Convert.ToInt32(n.date.Substring(0, 4)),  // year
//                    Convert.ToInt32(n.date.Substring(5, 2)),  // month
//                    Convert.ToInt32(n.date.Substring(8, 2)))  // day
//                    .Date == today && !n.IsSent)
//                .ToListAsync();

//            foreach (var notification in notifications)
//            {
//                try
//                {
//                    // Find all students in the specified department and semester
//                    var students = await dbContext.Students
//                        .Where(s => s.DepartmentId == notification.DepartmentId &&
//                                   s.Semester == notification.SemesterId &&
//                                   !string.IsNullOrEmpty(s.))
//                        .ToListAsync();

//                    if (!students.Any())
//                    {
//                        Console.WriteLine($"No students found for notification ID {notification.Id} (Department: {notification.DepartmentId}, Semester: {notification.SemesterId})");
//                        continue;
//                    }

//                    // Send notification to each student
//                    foreach (var student in students)
//                    {
//                        try
//                        {
//                            await notificationService.SendPushNotification(
//                                student.ExpoPushToken,
//                                notification.Title,
//                                notification.Message);
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine($"Failed to send notification to student {student.Id}: {ex.Message}");
//                            // Continue with next student even if one fails
//                        }
//                    }

//                    // Mark notification as sent only after attempting to send to all students
//                    notification.IsSent = true;
//                    dbContext.Update(notification);

//                    // Save changes after each notification to prevent data loss if something fails
//                    await dbContext.SaveChangesAsync();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error processing notification ID {notification.Id}: {ex.Message}");
//                    // Continue with next notification even if one fails
//                }
//            }
//        }
//    }
//}
