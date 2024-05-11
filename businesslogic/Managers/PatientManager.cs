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
            leerArchivo();
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
            escribirArchivo();
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
                escribirArchivo();
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
                escribirArchivo();
                return patientToRemove;
            }
            else
            {
                throw new KeyNotFoundException($"Patient with code {patientCodeToDelete} not found.");
            }
        }



        public void leerArchivo()
        {
            // Ruta del archivo a leer
            string rutaArchivo = "C://Users//hp//Documents//QUINTO_SEMESTRE//CERTIFICACION__I//repositorio-p2//Patients.txt";

            try
            {
                // Verificamos si el archivo existe
                if (File.Exists(rutaArchivo))
                {
                    // Usamos StreamReader para leer el archivo línea por línea
                    using (StreamReader lector = new StreamReader(rutaArchivo))
                    {
                        string linea;
                        while ((linea = lector.ReadLine()) != null)
                        {
                            // Dividimos la línea en sus datos separados por comas
                            string[] datos = linea.Split(",");

                            // Verificamos que hay suficientes datos para crear un paciente
                            if (datos.Length >= 4)
                            {
                                // Creamos un nuevo paciente con los datos de la línea
                                Patient paciente = new Patient
                                {
                                    Name = datos[0].Trim(), // Eliminamos los espacios alrededor del nombre
                                    LastName = datos[1].Trim(),
                                    CI = datos[2].Trim(),
                                    Code = datos[3].Trim()
                                };

                                _patients.Add(paciente);
                            }
                            else
                            {
                                Console.WriteLine("La línea no tiene suficientes datos para crear un paciente.");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El archivo no existe.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Ocurrió un error al leer el archivo: " + ex.Message);
            }
        }


        public void escribirArchivo()
        {
            // Ruta del archivo donde se escribirán los datos
            string rutaArchivo = "C://Users//hp//Documents//QUINTO_SEMESTRE//CERTIFICACION__I//repositorio-p2//Patients.txt";

            try
            {
                // Usamos StreamWriter para escribir en el archivo
                using (StreamWriter escritor = new StreamWriter(rutaArchivo))
                {
                    // Iteramos sobre cada paciente en la lista _patients
                    foreach (Patient paciente in _patients)
                    {
                        // Creamos una línea con los datos del paciente separados por comas
                        string linea = $"{paciente.Name},{paciente.LastName},{paciente.CI},{paciente.Code}";

                        // Escribimos la línea en el archivo
                        escritor.WriteLine(linea);
                    }
                }

                Console.WriteLine("Se han escrito los datos en el archivo correctamente.");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Ocurrió un error al escribir en el archivo: " + ex.Message);
            }
        }

    }
}
