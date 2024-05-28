using FindDoctor.Data;
using Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindDoctor.Models;

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
            
            PatientViewModel patientViewModel = new PatientViewModel
            {
                _PatientModel = new PatientModel(),
                DeartmentList = _applicationDbContext.Doctors.Where(n => n.DoctorId > 0).Select(
                    s=> new SelectListItem
                    {
                        Text = s.Department,
                        Value= s.DoctorId.ToString()
                    }
                    )
            };


           
                return View(patientViewModel);

           
        }


        [HttpPost]
        public async Task<IActionResult> ConsultationAsync(PatientViewModel patientModel, IFormFile? file)
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

                   
                    patientModel._PatientModel.ReportFile = "/images/reports/" + fileName;
                    _applicationDbContext.Customers.Add(patientModel._PatientModel);

                    await _applicationDbContext.SaveChangesAsync();
                }
                return RedirectToAction("List");
            }

            return View();
        }


        public IActionResult List()
        {
            var patients = _applicationDbContext.Customers
                .Include(p => p.DescriptionDetections) // Ensure related data is included
                .ToList();

            var departmentList = _applicationDbContext.Doctors
                .Where(n => n.DoctorId > 0)
                .Select(s => new SelectListItem
                {
                    Text = s.Department,
                    Value = s.DoctorId.ToString()
                }).ToList();

            var patientViewModels = patients.Select(patient => new PatientViewModel
            {
                _PatientModel = patient,
                DeartmentList = departmentList
            }).ToList();

            return View(patientViewModels);
        }




        [HttpGet]
        public IActionResult Update(int? id)
        {

            PatientViewModel patientViewModel = new PatientViewModel
            {
                _PatientModel = new PatientModel(),
                DeartmentList = _applicationDbContext.Doctors.Where(n => n.DoctorId > 0).Select(
                    s => new SelectListItem
                    {
                        Text = s.Department,
                        Value = s.DoctorId.ToString()
                    }
                    )
            };

            patientViewModel._PatientModel = _applicationDbContext.Customers
               .Include(p => p.DescriptionDetections)

               .FirstOrDefault(a => a.CustomerId == id);
            return View(patientViewModel);
           
        }



        [HttpPost]
        public async Task<IActionResult> Update(PatientViewModel? patientModel, IFormFile? file, bool? delete)
        {
            if (delete==true)
            {
                _applicationDbContext.Customers.Remove(patientModel._PatientModel);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            if (ModelState.IsValid)
            {
                var patient = await _applicationDbContext.Customers.FindAsync(patientModel._PatientModel.CustomerId);
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

                
                patient.Name = patientModel._PatientModel.Name;
                patient.Surname = patientModel._PatientModel.Surname;
                patient.Description = patientModel._PatientModel.Description;
                patient.DescriptionDetections = patientModel._PatientModel.DescriptionDetections;
                patient.DoctorId = patientModel._PatientModel.DoctorId;
                _applicationDbContext.Update(patient);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }

            return View(patientModel);
        }
    }
}
