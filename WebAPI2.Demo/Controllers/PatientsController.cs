using WebAPI2.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Web.Http.Cors;

namespace WebAPI2.Demo.Controllers
{
    [Authorize]
    //[EnableCors("", "*", "GET")]
    [EnableCors("http://localhost:56642", "*", "GET")]
    public class PatientsController : ApiController
    {
        IMongoCollection<Patient> _patients;
        public PatientsController()
        {
            _patients = PatientDB.Open();
        }
        
        public IEnumerable<Patient> Get()
        {

            return _patients.Find<Patient>(i => i.Name != "123").ToEnumerable();
        }
        
        public HttpResponseMessage Get(string Id)
        {

            var patient = _patients.Find<Patient>(i => i.Id == Id).FirstOrDefault();
            if (patient == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Patient Not Found");
            }
            return Request.CreateResponse(patient);
        }
        [Route("api/patients/{id}/medications")]
        public HttpResponseMessage GetMedication(string Id)
        {

            var patient = _patients.Find<Patient>(i => i.Id == Id).FirstOrDefault();
            if (patient == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Patient Not Found");
            }
            return Request.CreateResponse(patient.Medications);
        }
        //new way
        [Route("api/patients/{id}/ailments")]
        public IHttpActionResult GetAilments(string Id)
        {

            var patient = _patients.Find<Patient>(i => i.Id == Id).FirstOrDefault();
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient.Ailments);
        }
    }
}
