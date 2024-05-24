using FindDoctor.Data;
using FindDoctor.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindDoctor.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult List()
        {
            var ListOfPatient = _applicationDbContext.Customers.ToList();

            return View(ListOfPatient);
        }
        [HttpGet]
        public IActionResult Details(int? id, int? descriptionID)
        {
            var patient = _applicationDbContext.Customers.FirstOrDefault(a=>a.CustomerId==id);
            ViewBag.descriptionID=descriptionID;
            if (patient.DiseaseDetection == null)
            {
                patient.DiseaseDetection = new List<string>();
            }
            return View(patient);
        }
        [HttpPost]
        public IActionResult Details(PatientModel patientModel)
        {
            if (patientModel == null) { 
            return View(patientModel);
            }
            var patient = _applicationDbContext.Customers.FirstOrDefault(a => a.CustomerId == patientModel.CustomerId);
            patient.DiseaseDetection.Add(patientModel.DiseaseDetection[0]);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
