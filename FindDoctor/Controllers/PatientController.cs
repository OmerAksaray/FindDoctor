using FindDoctor.Data;
using FindDoctor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;

namespace FindDoctor.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

      
        public PatientController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
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
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string reportPath = Path.Combine(wwwRootPath, "images", "reports");

                   
                    if (!Directory.Exists(reportPath))
                    {
                        Directory.CreateDirectory(reportPath);
                    }

                    string fullPath = Path.Combine(reportPath, fileName);

                   
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                   
                    patientModel.ReportFile = "/images/reports/" + fileName;
                    _applicationDbContext.Customers.Add(patientModel);

                    await _applicationDbContext.SaveChangesAsync();
                }
                return RedirectToAction("List");
            }

            return View();
        }

      
        public IActionResult List()
        {
            var listOfPatients = _applicationDbContext.Customers.ToList();
            return View(listOfPatients);
        }

       
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _applicationDbContext.Customers
                .Include(p => p.DescriptionDetections)
                .FirstOrDefault(p => p.CustomerId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }



        [HttpPost]
        public async Task<IActionResult> Update(PatientModel? patientModel, IFormFile? file, bool? delete)
        {
            if (delete==true)
            {
                _applicationDbContext.Customers.Remove(patientModel);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            if (ModelState.IsValid)
            {
                var patient = await _applicationDbContext.Customers.FindAsync(patientModel.CustomerId);
                if (patient == null)
                {
                    return NotFound();
                }

                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string reportPath = Path.Combine(wwwRootPath, "images", "reports");

                  
                    if (!Directory.Exists(reportPath))
                    {
                        Directory.CreateDirectory(reportPath);
                    }

                    string fullPath = Path.Combine(reportPath, fileName);

                  
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    
                    patient.ReportFile = "/images/reports/" + fileName;
                }

                
                patient.Name = patientModel.Name;
                patient.Surname = patientModel.Surname;
                patient.Description = patientModel.Description;
                patient.DescriptionDetections = patientModel.DescriptionDetections;

                _applicationDbContext.Update(patient);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }

            return View(patientModel);
        }
    }
}
