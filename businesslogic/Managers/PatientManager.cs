using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPB.BusinessLogic.Models;

namespace UPB.BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private readonly IConfiguration _configuration;

        public PatientManager()
        {
            _patients = new List<Patient>();
            _patients.Add(new Patient()
            {
                Name = "Maria",
                Code = "20087",
                Id = 1
            });

            Patient student = new Patient()
            {
                Name = "Jose",
                Code = "200337",
                Id = 2
            };

            Patient student2 = new Patient()
            {
                Name = "Pepe",
                Code = "300556",
                Id = 3
            };
        }
        public List<Patient> GetAll()
        {
            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("dbConnection").Value;
            Console.WriteLine(connectionString);
            return _patients;
        }

        public Patient GetStudentById(int id)
        {
            Patient foundStudent = _patients.Find(x => x.Id == id);
            return foundStudent;
        }

        public Patient CreateStudent(Patient student)
        {
            Patient createdStudent = new Patient()
            {
                Name = student.Name,
                Code = student.Code,
                Id = student.Id
            };
            _patients.Add(createdStudent);
            return createdStudent;
        }

        public Patient UpdateStudent(int id, Patient studentToUpdate)
        {
            // logica de buscar y actualizar
            throw new NotImplementedException();
        }

        public Patient DeleteStudent(int studentIdToDelete)
        {
            // logica  de buscar y borrar
            throw new NotImplementedException();
        }
    }
}
