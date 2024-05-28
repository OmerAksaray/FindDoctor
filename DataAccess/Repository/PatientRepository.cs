using DataAccess.Repository.IRepository;
using FindDoctor.Data;
using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PatientRepository : Repository<PatientModel>, IPatientRepository
    {
        private ApplicationDbContext _db {  get; set; }
        public PatientRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(PatientModel patient)
        {
            _db.Customers.Update(patient);
        }
    }
}
