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
        public PatientController(PatientManager studentManager)
        {
            _patientManager = studentManager;
        }

        // GET; api/<StudentController>
        [HttpGet]
        public List<Patient> Get()
        {
            Log.Information("Initialize Stud3entController");
            return _patientManager.GetAll();
        }

        // GET: api/<StudentController>/5
        [HttpGet]
        [Route("{id}")]
        public Patient Get(int id)
        {
            return _patientManager.GetStudentById(id);
        }

        // POST api/<StudentController>
        [HttpPost]
        public void Post([FromBody] Patient value)
        {
            _patientManager.CreateStudent(value);
        }

        // PUT:api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Patient value)
        {
            _patientManager.UpdateStudent(id, value);
        }

        // DELETE:api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientManager.DeleteStudent(id);
        }
    }
}
