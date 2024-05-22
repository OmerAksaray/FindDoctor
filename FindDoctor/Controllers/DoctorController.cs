using FindDoctor.Data;
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
        public IActionResult Details(int? id, int? descriptionID)
        {
            var patient = _applicationDbContext.Customers.FirstOrDefault(a=>a.CustomerId==id);
            ViewBag.descriptionID=descriptionID;
            return View(patient);
        }
    }
}
