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
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Patient value)
        {
            try
            {
                _patientManager.UpdatePatient(id, value);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, $"Patient with ID {id} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating a patient.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // DELETE:api/<PatientController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _patientManager.DeletePatient(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(ex, $"Patient with ID {id} not found.");
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
