using Notification.Domain.Entities;
using Notification.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Notification.Infrastructure.Persistence
{
    public static class NotificationSeed
    {
        public static async Task Initialize(NotificationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            if (await dbContext.Notifications.AnyAsync())
                return;

            var patientId = "c5a59d9e-02b4-4c72-a5e4-2c76b6d53133";
            var doctorId = "8d7c6b5f-09f4-4b22-a6a8-3b7dbe4f92f2";

            var seedNotifications = new List<Notification.Domain.Entities.Notification>
            {
               
                new Notification.Domain.Entities.Notification { UserId = patientId, Title = "Appointment Reminder", Message = "You have an appointment with Dr. Smith tomorrow at 10:00 AM.", CreatedAt = DateTime.UtcNow.AddHours(-3), IsRead = false, Icon = "calendar.png" },
                new Notification.Domain.Entities.Notification { UserId = patientId, Title = "Lab Results Ready", Message = "Your blood test results are now available.", CreatedAt = DateTime.UtcNow.AddHours(-5), IsRead = false, Icon = "lab.png" },
                new Notification.Domain.Entities.Notification { UserId = patientId, Title = "Prescription Update", Message = "Your prescription for blood pressure medication has been renewed.", CreatedAt = DateTime.UtcNow.AddDays(-1), IsRead = true, Icon = "prescription.png" },
                new Notification.Domain.Entities.Notification { UserId = patientId, Title = "Message from Doctor", Message = "Dr. Smith sent you a message regarding your recent visit.", CreatedAt = DateTime.UtcNow.AddHours(-10), IsRead = false, Icon = "message.png" },
                new Notification.Domain.Entities.Notification { UserId = patientId, Title = "Health Tip", Message = "Remember to drink 8 glasses of water daily!", CreatedAt = DateTime.UtcNow.AddDays(-2), IsRead = true, Icon = "health.png" },

              
                new Notification.Domain.Entities.Notification { UserId = doctorId, Title = "New Appointment Scheduled", Message = "Patient John Doe booked an appointment for tomorrow at 2:00 PM.", CreatedAt = DateTime.UtcNow.AddHours(-2), IsRead = false, Icon = "calendar.png" },
                new Notification.Domain.Entities.Notification { UserId = doctorId, Title = "Lab Results Reviewed", Message = "You reviewed the lab results for patient Jane Doe.", CreatedAt = DateTime.UtcNow.AddHours(-6), IsRead = true, Icon = "lab.png" },
                new Notification.Domain.Entities.Notification { UserId = doctorId, Title = "Message from Patient", Message = "Patient Alex sent you a message regarding symptoms.", CreatedAt = DateTime.UtcNow.AddHours(-4), IsRead = false, Icon = "message.png" },
                new Notification.Domain.Entities.Notification { UserId = doctorId, Title = "Schedule Reminder", Message = "You have 3 appointments scheduled for today.", CreatedAt = DateTime.UtcNow.AddHours(-1), IsRead = false, Icon = "calendar.png" },
                new Notification.Domain.Entities.Notification { UserId = doctorId, Title = "Prescription Approval", Message = "Your prescription approval for patient Mary has been completed.", CreatedAt = DateTime.UtcNow.AddDays(-1), IsRead = true, Icon = "prescription.png" },
            };

            await dbContext.Notifications.AddRangeAsync(seedNotifications);
            await dbContext.SaveChangesAsync();
        }
    }
}
