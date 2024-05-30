
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FindDoctor.Data;
using Models;
using FindDoctor.Models;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;

namespace FindDoctor.Areas.doctor.Controllers
{
    [Area("Doctor")]
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctor;
        private readonly IPatientRepository _patient;
        private readonly IDescriptionDetectionRepository _detection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DoctorController(IDoctorRepository doctor, IPatientRepository patient, IDescriptionDetectionRepository detection,IWebHostEnvironment webHostEnvironment)
        {
            _doctor = doctor;
            _patient = patient;
            _detection = detection;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult List()
        {
            var ListOfPatient = _patient.GetAll();
            var ListOfResponse = _detection.GetAll()
                .ToDictionary(response => response.PatientId);
            ViewBag.ListOfResponse = ListOfResponse;
            return View(ListOfPatient);
        }


        [HttpGet]
        public IActionResult Details(int? id)
        {

            var patient = _patient.Get(p => p.CustomerId == id);
            if (patient != null)
            {
                var doctorDetection = _detection.Get(p => p.PatientId == id);
                if (doctorDetection != null)
                {
                    ViewBag.DoctorResponse = doctorDetection.DiseaseDetection;
                }
                else
                {
                    ViewBag.DoctorResponse = "No doctor response available.";
                }

                return View(patient);
            }
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, string diseaseDetection)
        {
            var patient = _patient.Get(p => p.CustomerId == id);
            var detection = _detection.Get(dd => dd.PatientId == id && dd.DoctorId == 1);

            if (patient != null && detection != null)
            {
                detection.DiseaseDetection = diseaseDetection;
                _detection.Update(detection);
            }
            else
            {
                if (patient == null)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    var newDetection = new PatientDescriptionDetection
                    {
                        PatientId = id,
                        DoctorId = 1,
                        Description = patient.Description,
                        DiseaseDetection = diseaseDetection
                    };
                    if (patient.DescriptionDetections == null)
                    {
                        patient.DescriptionDetections = new List<PatientDescriptionDetection>();
                    }
                    patient.DescriptionDetections.Add(newDetection);
                    _patient.Update(patient);
                }
            }

            _detection.Save();
            _patient.Save();

            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deleted=_patient.Get(p=>p.CustomerId == id);
            if ((deleted==null))
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                deleted.ReportFile.TrimStart('\\', '/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                
                System.IO.File.Delete(oldImagePath);
            }
            _patient.Remove(id);
            _patient.Save();
            return Json(new { success = true, message = "Patient deleted successfully." });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<PatientModel> obj = _patient.GetAll(includeProperties: "Doctor").ToList();
            return Json(new { data = obj });

        }
        #endregion 
    }
}