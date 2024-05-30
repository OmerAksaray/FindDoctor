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
using DataAccess.Repository.IRepository;
using System.Numerics;

namespace FindDoctor.Areas.user.Controllers
{
    [Area("User")]
    public class PatientController : Controller
    {
        private readonly IDoctorRepository _doctor;
        private readonly IPatientRepository _patient;
        private readonly IDescriptionDetectionRepository _detection;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public PatientController(IWebHostEnvironment webHostEnvironment, IDoctorRepository doctor, IPatientRepository patient, IDescriptionDetectionRepository detection)
        {
            _doctor = doctor;
            _patient = patient;
            _detection = detection;
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpGet]
        public IActionResult Consultation()
        {

            PatientViewModel patientViewModel = new PatientViewModel
            {
                _PatientModel = new PatientModel(),
                DepartmentList = _doctor.GetAll().Select(
                 s => new SelectListItem
                 {
                     Text = s.Department,
                     Value = s.DoctorId.ToString()
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
                    _patient.Add(patientModel._PatientModel);

                    _patient.Save();
                }
                return RedirectToAction("List");
            }

            return View();
        }


        public IActionResult List()
        {
            //var patients = _applicationDbContext.Customers
            //    .Include(p => p.DescriptionDetections) // Ensure related data is included
            //    .ToList();

            var patients = _patient.GetAll();
            var ListOfResponse = _detection.GetAll()
               .ToDictionary(response => response.PatientId);
            ViewBag.ListOfResponse = ListOfResponse;


            return View(patients);
        }




        [HttpGet]
        public IActionResult Update(int? id)
        {

            PatientViewModel patientViewModel = new PatientViewModel
            {
                _PatientModel = new PatientModel(),
                DepartmentList = _doctor.GetAll().Select(
                    s => new SelectListItem
                    {
                        Text = s.Department,
                        Value = s.DoctorId.ToString()
                    }
                    )
            };

            patientViewModel._PatientModel = _patient
               .Get(a => a.CustomerId == id);
            if (_detection.Get(p => p.PatientId == id) != null)
            {

                ViewBag.DescriptionDetection = _detection.Get(p => p.PatientId == id).DiseaseDetection;
            }
            else
            {
                ViewBag.DescriptionDetection = "Not reccomendation yet.";
            }
            return View(patientViewModel);

        }



        [HttpPost]
        public async Task<IActionResult> Update(PatientViewModel? patientModel, IFormFile? file, bool? delete)
        {
            if (delete == true)
            {
                _patient.Remove(patientModel._PatientModel.CustomerId);
                _patient.Save();
                return RedirectToAction("List");
            }
            if (ModelState.IsValid)
            {
                var patient = _patient.Get(p => p.CustomerId == patientModel._PatientModel.CustomerId);
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
                _patient.Update(patient);
                _patient.Save();

                return RedirectToAction("List");
            }

            return View(patientModel);
        }
    }
}
