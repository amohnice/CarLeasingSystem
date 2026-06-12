using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarLeasingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        public string? CustomerName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Foreign Key to the Car
        public int CarId { get; set; }
        
        [ForeignKey("CarId")]
        [ValidateNever]
        public Car Car { get; set; }
    }
}
