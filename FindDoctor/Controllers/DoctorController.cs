using FindDoctor.Data;
using FindDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            var ListOfPatient = _applicationDbContext.Customers
                .Include(p => p.DescriptionDetections)
                .ToList();
            return View(ListOfPatient);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _applicationDbContext.Customers
                .Include(p => p.DescriptionDetections)
               
                .FirstOrDefault(a => a.CustomerId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, string diseaseDetection)
        {
           
           
            var patientDescribe = _applicationDbContext.Customers
                .Include(p => p.DescriptionDetections)
                .FirstOrDefault(dd => dd.CustomerId == id);

            if (patientDescribe == null)
            {
                return NotFound("Patient not found");
            }

            var descriptionDetection = patientDescribe.DescriptionDetections
                .FirstOrDefault(dd => dd.PatientId == id && dd.DoctorId == 1);

            if (descriptionDetection != null)
            {
                descriptionDetection.DiseaseDetection = diseaseDetection;
                _applicationDbContext.DescriptionDetections.Update(descriptionDetection);
            }
            else
            {
                var newDetection = new PatientDescriptionDetection
                {
                    PatientId = id,
                    DoctorId = 1,
                    Description = patientDescribe.Description,
                    DiseaseDetection = diseaseDetection
                };
                _applicationDbContext.DescriptionDetections.Add(newDetection);
            }

            _applicationDbContext.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
