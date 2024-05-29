using System.ComponentModel.DataAnnotations;

namespace FindDoctor.Models
{
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string Surname { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string DiseaseDetection { get; set; }
    }
}
