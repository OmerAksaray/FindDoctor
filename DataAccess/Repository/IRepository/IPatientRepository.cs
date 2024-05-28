using FindDoctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IPatientRepository:IRepository<PatientModel>
    {
        void Update(PatientModel patient);
        void Save();
    }
}
