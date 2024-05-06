using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using UPB.BusinessLogic.Models;
using UPB.BusinessLogic.Managers;
using Serilog;


namespace NewAppVS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private List<Patient> _patients;

        // Constructor
        private PatientManager _patientManager;
        public PatientController(PatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        // GET; api/<PatientController>
        [HttpGet]
        public List<Patient> Get()
        {
            Log.Information("Initialize Pati3ntController");
            return _patientManager.GetAll();
        }

        // GET: api/<PatientController>/5
        [HttpGet]
        [Route("{id}")]
        public Patient Get(int id)
        {
            return _patientManager.GetPatientById(id);
        }

        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody] Patient value)
        {
            _patientManager.CreatePatient(value);
        }

        // PUT:api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Patient value)
        {
            _patientManager.UpdatePatient(id, value);
        }

        // DELETE:api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientManager.DeletePatient(id);
        }
    }
}
