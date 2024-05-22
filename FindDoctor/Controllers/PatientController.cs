using FindDoctor.Data;
using FindDoctor.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindDoctor.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PatientController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public IActionResult Consultation()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Consultation(PatientModel patientModel)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Customers.Add(patientModel);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("List");
            }

            return View();
        }
        
        public IActionResult List()
        {
            var ListOfPatient=_applicationDbContext.Customers.ToList();

            return View(ListOfPatient);
        }

      
    }
}
