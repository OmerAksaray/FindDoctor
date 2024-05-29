using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindDoctor.Models
{
    public class PatientModel
    {
        [Key]
        public int CustomerId { get; set; }  // Birincil anahtar

        public string? Name { get; set; }  // Hasta adı
        public string? Surname { get; set; }  // Hasta soyadı
        public string? ReportFile { get; set; }  // Rapor dosyası yolu

        [Required]
        public string? Description { get; set; }  // Hastalık açıklaması

        [ValidateNever]
        public ICollection<PatientDescriptionDetection> DescriptionDetections { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        [ValidateNever]
        public DoctorModel? Doctor { get; set; }
    }
}
