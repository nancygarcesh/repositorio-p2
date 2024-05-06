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
                Name = "Nancy",
                Code = "20087",
                Id = 1
            });

            Patient patient = new Patient()
            {
                Name = "Elizabeth",
                Code = "201137",
                Id = 2
            };

            Patient patient2 = new Patient()
            {
                Name = "Emma",
                Code = "110556",
                Id = 3
            };
        }
        public List<Patient> GetAll()
        {
            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("dbConnection").Value;
            Console.WriteLine(connectionString);
            return _patients;
        }

        public Patient GetPatientById(int id)
        {
            Patient foundPatient = _patients.Find(x => x.Id == id);
            return foundPatient;
        }

        public Patient CreatePatient(Patient student)
        {
            Patient createdPatient = new Patient()
            {
                Name = student.Name,
                Code = student.Code,
                Id = student.Id
            };
            _patients.Add(createdPatient);
            return createdPatient;
        }

        public Patient UpdatePatient(int id, Patient patientToUpdate)
        {
            Patient existingPatient = _patients.FirstOrDefault(p => p.Id == id);
            if (existingPatient != null)
            {
                existingPatient.Name = patientToUpdate.Name;
                existingPatient.Code = patientToUpdate.Code;


                return existingPatient;
            }
            else
            {

                throw new KeyNotFoundException($"Patient with ID {id} not found.");
            }
        }

        public Patient DeletePatient(int patientIdToDelete)
        {
            Patient patientToRemove = _patients.FirstOrDefault(p => p.Id == patientIdToDelete);
            if (patientToRemove != null)
            {
                _patients.Remove(patientToRemove);
                return patientToRemove;
            }
            else
            {

                throw new KeyNotFoundException($"Patient with ID {patientIdToDelete} not found.");
            }
        }
    }
}
