using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace FindDoctor.Models
{
    public class PatientViewModel
    {

        public PatientModel _PatientModel { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> DeartmentList { get; set; }
    }
}
