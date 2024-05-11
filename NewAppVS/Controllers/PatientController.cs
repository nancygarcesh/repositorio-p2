using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

using UPB.BusinessLogic.Models;
using UPB.BusinessLogic.Managers;
using Serilog;
using System.Text;
using Newtonsoft.Json;


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
        public Patient Get(string code)
        {
            return _patientManager.GetPatientByCode(code);
        }

        // POST api/<PatientController>
        [HttpPost]
        public IActionResult Post([FromBody] Patient value)
        {
            try
            {
                _patientManager.CreatePatient(value);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating a patient.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // PUT:api/<PatientController>/5
        [HttpPut("{code}")]
        public IActionResult Put(string code, [FromBody] Patient value)
        {
            try
            {
                _patientManager.UpdatePatient(code, value);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, $"Patient with ID {code} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating a patient.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // DELETE:api/<PatientController>/5
        [HttpDelete("{code}")]
        public IActionResult Delete(string code)
        {
            try
            {
                _patientManager.DeletePatient(code);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, $"Patient with ID {code} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting a patient.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}
