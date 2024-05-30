using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FindDoctor.Models
{
    public class PatientModel
    {
        [Key]
        public int CustomerId { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? ReportFile { get; set; }

        [Required]
        public string? Description { get; set; }

        [ValidateNever]
        public ICollection<PatientDescriptionDetection> DescriptionDetections { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        [ValidateNever]
        public DoctorModel? Doctor { get; set; }
    }
}