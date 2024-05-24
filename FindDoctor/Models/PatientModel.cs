using System.ComponentModel.DataAnnotations;

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
        public List<string> Description { get; set; }
        
        public List<string> DiseaseDetection{ get; set; }=new List<string>();


      
    }
}
