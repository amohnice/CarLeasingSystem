using System.ComponentModel.DataAnnotations;

namespace CarLeasingSystem.Models
{
    public class Car
    {
        public int Id { get; set; } // Primary Key

        [Required]
        public string Make { get; set; } // e.g., Toyota, Honda

        [Required]
        public string Model { get; set; } // e.g., Camry, Civic

        [Required]
        public string LicensePlate { get; set; }

        public decimal DailyRate { get; set; }

        public bool IsAvailable { get; set; } = true; // Default to true
    }
}
