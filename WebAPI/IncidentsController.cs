using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DashOwl.DAL;
using DashOwl.Models;
using DashOwl.WebAPI;
using System.Linq.Expressions;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json.Linq;
using AutoMapper;

namespace DashOwl
{
    /// <summary>
    /// Incident entity API controller
    /// </summary>
    public class IncidentsController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();
        private readonly IIncidentRepository _incidentRepo;

        public IncidentsController(IIncidentRepository repository)
        {
            this._incidentRepo = repository;
        }

        public IncidentsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// Gets list of all Incidents
        /// </summary>
        /// <returns>A list of Incidents</returns>
        public async Task<IHttpActionResult> GetIncidents()
        {
            IList<Incident> incidents = _incidentRepo.GetAllIncidents();
            IList<IncidentDetailsDto> incidentsDto = Mapper.Map<IList<Incident>, IList<IncidentDetailsDto>>(incidents);

            if(incidentsDto == null)
            {
                return NotFound();
            }
            return Ok(incidentsDto);
        }

        /// <summary>
        /// Gets Incident by ID
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>Incident</returns>
        [ResponseType(typeof(IncidentDetailsDto))]
        public async Task<IHttpActionResult> GetIncident(int id)
        {
            Incident incident = _incidentRepo.GetSingle(id);
            IncidentDetailsDto incidentDto = Mapper.Map<Incident, IncidentDetailsDto>(incident);

            if (incidentDto == null)
            {
                return NotFound();
            }

            return Ok(incidentDto);
        }

        /// <summary>
        /// Updates Incident.
        /// </summary>
        /// <param name="id">Incident ID</param>
        /// <param name="incident">Modified Incident. JSON sample: {"ID":1,"CreationDate":"2015-05-01T00:00:00","Description":"Some dummy description 1 test123"}</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIncident([FromBody] IncidentDetailsDto incidentDto)
        {
            Incident incident = Mapper.Map<IncidentDetailsDto, Incident>(incidentDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _incidentRepo.Edit(incident);
            _incidentRepo.Save();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates Incident.
        /// </summary>
        /// <param name="incidentDetailsDto">A new Incident. JSON sample:{"Description":"Some dummy description 1 test123","CreationDate":"2014-12-31T00:00:00"}</param>
        /// <returns>Created Incident</returns>
        [ResponseType(typeof(IncidentDetailsDto))]
        public async Task<IHttpActionResult> PostIncident([FromBody]IncidentDetailsDto incidentDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var incident = Mapper.Map<IncidentDetailsDto, Incident>(incidentDetailsDto);

            _incidentRepo.Add(incident);
            _incidentRepo.Save();

            incidentDetailsDto = Mapper.Map<Incident, IncidentDetailsDto>(incident);

            return Ok(incidentDetailsDto);
        }

        /// <summary>
        /// Deteles Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>Ok, if deleted successfully</returns>
        [ResponseType(typeof(IncidentDetailsDto))]
        public async Task<IHttpActionResult> DeleteIncident(int id)
        {
            Incident incident = _incidentRepo.GetSingle(id);
            var incidentDto = Mapper.Map<Incident, IncidentDetailsDto>(incident);

            if (incident == null)
            {
                return NotFound();
            }

            _incidentRepo.Delete(incident);
            _incidentRepo.Save();

            return Ok(incidentDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IncidentExists(int id)
        {
            return db.Incidents.Count(e => e.ID == id) > 0;
        }
    }
}