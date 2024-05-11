using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UPB.BusinessLogic.Models;

namespace UPB.BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PatientManager(IConfiguration configuration, HttpClient httpClient)
        {
            _patients = new List<Patient>();
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public List<Patient> GetAll()
        {
            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("dbConnection").Value;
            Console.WriteLine(connectionString);
            return _patients;
        }

        public Patient GetPatientByCode(string code)
        {
            Patient foundPatient = _patients.Find(x => x.Code == code);
            return foundPatient;
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            // Generamos el código de paciente llamando al servicio del Practice 3
            string patientCode = await GeneratePatientCodeAsync(patient.Name, patient.LastName, patient.CI);

            // Asignamos el código de paciente generado
            patient.Code = patientCode;

            // Agregamos el paciente a la lista
            _patients.Add(patient);

            // Retornamos el paciente creado
            return patient;
        }

        public async Task<string> GeneratePatientCodeAsync(string name, string lastName, string ci)
        {
            // Construimos la URL del servicio en Practice 3
            string practice3BaseUrl = _configuration.GetSection("Practice3BaseUrl").Value;
            string generateCodeEndpoint = "api/PatientController1";

            // Creamos el objeto que contiene la información del paciente
            var patientInfo = new { Name = name, LastName = lastName, CI = ci };

            // Serializamos el objeto a JSON
            var jsonPatientInfo = JsonConvert.SerializeObject(patientInfo);

            // Creamos el contenido de la solicitud HTTP
            var content = new StringContent(jsonPatientInfo, System.Text.Encoding.UTF8, "application/json");

            // Realizamos la solicitud HTTP POST al servicio de Practice 3
            var response = await _httpClient.PostAsync($"{practice3BaseUrl}/{generateCodeEndpoint}", content);

            // Verificamos si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leemos la respuesta como texto
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserializamos la respuesta para obtener el código de paciente
                var responseObject = JsonConvert.DeserializeAnonymousType(responseContent, new { PatientCode = "" });

                // Retornamos el código de paciente generado
                return responseObject.PatientCode;
            }
            else
            {
                // Si la solicitud no fue exitosa, lanzamos una excepción o manejamos el error según sea necesario
                throw new HttpRequestException($"Error al generar el código de paciente: {response.StatusCode}");
            }
        }

        public Patient UpdatePatient(string code, Patient patientToUpdate)
        {
            Patient existingPatient = _patients.FirstOrDefault(p => p.Code == code);
            if (existingPatient != null)
            {
                existingPatient.Name = patientToUpdate.Name;
                existingPatient.Code = patientToUpdate.Code;
                return existingPatient;
            }
            else
            {
                throw new KeyNotFoundException($"Patient with code {code} not found.");
            }
        }

        public Patient DeletePatient(string patientCodeToDelete)
        {
            Patient patientToRemove = _patients.FirstOrDefault(p => p.Code == patientCodeToDelete);
            if (patientToRemove != null)
            {
                _patients.Remove(patientToRemove);
                return patientToRemove;
            }
            else
            {
                throw new KeyNotFoundException($"Patient with code {patientCodeToDelete} not found.");
            }
        }
    }
}
