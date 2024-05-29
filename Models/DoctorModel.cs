using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FindDoctor.Models
{
    public class DoctorModel
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Department { get; set; }
        [ValidateNever]
        public ICollection<PatientDescriptionDetection> DescriptionDetections { get; set; }

        public virtual ICollection<PatientModel>? Patients { get; set; }
    }
}
