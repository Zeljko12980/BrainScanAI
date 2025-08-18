using System;

namespace Patient.Application.Dtos
{
    public class CreatePatientDto
    {
        // Osnovni podaci pacijenta
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Jmbg { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // Kontakt informacije
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Kontakt u hitnim slučajevima
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // Dodatni medicinski podaci
        public string BloodType { get; set; } = string.Empty;

        // Polja za autentifikaciju i autorizaciju
        public Guid UserId { get; set; }  // GUID koji se koristi kao ID korisnika
        public string Username { get; set; } = string.Empty;  // korisničko ime
        public string Password { get; set; } = string.Empty;  // lozinka
        public string ProfilePictureUrl { get; set; } = string.Empty; // URL profilne slike
    }
}
