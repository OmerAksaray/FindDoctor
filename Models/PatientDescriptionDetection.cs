using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindDoctor.Models
{
    public class PatientDescriptionDetection
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PatientModel")]
        public int PatientId { get; set; }
        public PatientModel Patient { get; set; }

        [ForeignKey("DoctorModel")]
        public int DoctorId { get; set; }
        public DoctorModel Doctor { get; set; }

        public string? Description { get; set; }
        public string? DiseaseDetection { get; set; }
    }
}