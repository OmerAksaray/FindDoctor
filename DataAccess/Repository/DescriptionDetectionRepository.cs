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
    public class DescriptionDetectionRepository: Repository<PatientDescriptionDetection>, IDescriptionDetectionRepository
    {
        private ApplicationDbContext _db { get; set; }
        public DescriptionDetectionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(PatientDescriptionDetection patient)
        {
            _db.DescriptionDetections.Update(patient);
        }
    }
}
