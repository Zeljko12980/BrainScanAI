﻿namespace Patient.Application.Dtos
{
    public class MedicalEventDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
