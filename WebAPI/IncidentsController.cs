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

namespace DashOwl
{
    /// <summary>
    /// Incident entity API controller
    /// </summary>
    public class IncidentsController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();

        List<IncidentDetailsDto> products = new List<IncidentDetailsDto>();

        public IncidentsController(List<IncidentDetailsDto> products)
        {
            this.products = products;
        }
        
        public IncidentsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// Gets list of all Incidents
        /// </summary>
        /// <returns>A list of Incidents</returns>
        public IList<IncidentDetailsDto> GetIncidents()
        {
            return db.Incidents.Select(p => new IncidentDetailsDto
                {
                    ID = p.ID,
                    CreationDate = p.CreationDate,
                    Description = p.Description,
                    MediaAssets = p.MediaAssets.Select(m => new MediaAssetDetailsDto
                    {
                        ID = m.ID,
                        CreationDate = m.CreationDate,
                        ExternalURL = m.ExternalURL,
                        ServerURL = m.ServerURL
                    }).ToList(),
                    Vehicles = p.Vehicles.Select(v => new VehicleDetailsDto
                    {
                        ID = v.ID,
                        PlateNumber = v.PlateNumber
                    }).ToList()
                }).ToList();
        }

        /// <summary>
        /// Gets Incident by ID
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>Incident</returns>
        [ResponseType(typeof(IncidentDetailsDto))]
        public async Task<IHttpActionResult> GetIncident(int id)
        {
            IncidentDetailsDto incident = await db.Incidents.Include(i => i.MediaAssets).Select(p => new IncidentDetailsDto
                {
                    ID = p.ID,
                    CreationDate = p.CreationDate,
                    Description = p.Description,
                    MediaAssets = p.MediaAssets.Select(m => new MediaAssetDetailsDto
                    {
                        ID = m.ID,
                        CreationDate = m.CreationDate,
                        ExternalURL = m.ExternalURL,
                        ServerURL = m.ServerURL
                    }).ToList(),
                    Vehicles = p.Vehicles.Select(v => new VehicleDetailsDto
                    {
                        ID = v.ID,
                        PlateNumber = v.PlateNumber
                    }).ToList()
                })
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (incident == null)
            {
                return NotFound();
            }

            return Ok(incident);
        }

        /// <summary>
        /// Updates Incident.
        /// </summary>
        /// <param name="id">Incident ID</param>
        /// <param name="incident">Modified Incident</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIncident(int id, [FromBody] Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           db.Incidents.Attach(incident);            
           db.Entry(incident).Property(x => x.Description).IsModified = true;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates Incident.
        /// </summary>
        /// <param name="incidentDetailsDto">A new Incident</param>
        /// <returns>Created Incident</returns>
        [ResponseType(typeof(IncidentDetailsDto))]
        public async Task<IHttpActionResult> PostIncident([ModelBinder]IncidentDetailsDto incidentDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Incident incident = new Incident()
            {
                CreationDate = DateTime.Now,
                Description = incidentDetailsDto.Description
            };
            
            db.Incidents.Add(incident);

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = incident.ID }, incident);
        }

        /// <summary>
        /// Deteles Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>Ok, if deleted successfully</returns>
        [ResponseType(typeof(Incident))]
        public async Task<IHttpActionResult> DeleteIncident(int id)
        {
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            db.Incidents.Remove(incident);
            await db.SaveChangesAsync();

            return Ok();
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