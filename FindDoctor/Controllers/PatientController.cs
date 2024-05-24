using FindDoctor.Data;
using FindDoctor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FindDoctor.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironmet;
        public PatientController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironmet)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironmet = webHostEnvironmet;
        }
        [HttpGet]
        public IActionResult Consultation()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConsultationAsync(PatientModel patientModel, IFormFile? file)
            

        {
            if (ModelState.IsValid)
            {
                
                string wwwRootPath = _webHostEnvironmet.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string studentPath = Path.Combine(wwwRootPath, @"images/reports");

                    using (var fileStream = new FileStream(Path.Combine(studentPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    };

                    patientModel.ReportFile = "/images/students/" + fileName;
                    _applicationDbContext.Customers.Add(patientModel);

                    _applicationDbContext.SaveChanges();
                }
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
