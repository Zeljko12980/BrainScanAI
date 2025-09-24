using Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointment.Infrastructure.Persistence
{
    public static class AppointmentSeed
    {
        public static void Initialize(AppointmentDbContext context)
        {
            if (!context.Appointments.Any())
            {
                var patientId = Guid.Parse("c5a59d9e-02b4-4c72-a5e4-2c76b6d53133");
                var doctorId = Guid.Parse("8d7c6b5f-09f4-4b22-a6a8-3b7dbe4f92f2");

                var appointments = new List<Appointment.Domain.Entities.Appointment>
                {
                    new Appointment.Domain.Entities.Appointment(
                        patientId,
                        doctorId,
                        DateTime.UtcNow.AddHours(2).AddMinutes(20 - DateTime.UtcNow.Minute % 20),
                        "Oncology Room 1",
                        "Dr. John Smith",
                        "Oncology",
                        "Alice Johnson",
                        "Initial consultation"
                    ),
                    new Appointment.Domain.Entities.Appointment(
                        patientId,
                        doctorId,
                        DateTime.UtcNow.AddHours(4).AddMinutes(20 - DateTime.UtcNow.Minute % 20),
                        "Oncology Room 1",
                        "Dr. John Smith",
                        "Oncology",
                        "Alice Johnson",
                        "Chemotherapy follow-up"
                    ),
                    new Appointment.Domain.Entities.Appointment(
                        patientId,
                        doctorId,
                        DateTime.UtcNow.AddDays(1).AddHours(1).AddMinutes(20 - DateTime.UtcNow.Minute % 20),
                        "Oncology Room 2",
                        "Dr. John Smith",
                        "Oncology",
                        "Alice Johnson",
                        "Radiation therapy assessment"
                    ),
                    new Appointment.Domain.Entities.Appointment(
                        patientId,
                        doctorId,
                        DateTime.UtcNow.AddDays(1).AddHours(3).AddMinutes(20 - DateTime.UtcNow.Minute % 20),
                        "Oncology Room 3",
                        "Dr. John Smith",
                        "Oncology",
                        "Alice Johnson",
                        "Bloodwork results review"
                    ),
                    new Appointment.Domain.Entities.Appointment(
                        patientId,
                        doctorId,
                        DateTime.UtcNow.AddDays(2).AddHours(2).AddMinutes(20 - DateTime.UtcNow.Minute % 20),
                        "Oncology Room 4",
                        "Dr. John Smith",
                        "Oncology",
                        "Alice Johnson",
                        "Follow-up after treatment"
                    )
                };

                context.Appointments.AddRange(appointments);
                context.SaveChanges();
            }
        }
    }
}
