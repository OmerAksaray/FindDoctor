using DataAccess.Repository.IRepository;
using FindDoctor.Data;
using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    internal class DoctorRepository : Repository<DoctorModel>, IDoctorRepository
    {
        private ApplicationDbContext _db { get; set; }


        public DoctorRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(DoctorModel model)
        {
            _db.Doctors.Update(model);
        }
    }
}
