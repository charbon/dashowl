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

namespace DashOwl.WebAPI
{
    /// <summary>
    /// Vehicle entity API controller
    /// </summary>
    public class VehiclesController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();

        // GET: api/Vehicles
        /// <summary>
        /// Gets a list of Vehicles related to Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>A list of Vehicles</returns>
        [Route("api/incidents/{id}/vehicles")]
        public async Task<IHttpActionResult> GetVehicles(int id)
        {
            var vehicle = await db.Vehicles.Select(p => new VehicleDetailsDto
            {
                ID = p.ID,                
                PlateNumber = p.PlateNumber,
                IncidentID = p.IncidentID                
            })
                .Where(i => i.IncidentID == id).ToListAsync();


            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // GET: api/Vehicles/5
        /// <summary>
        /// Gets Vehicle by ID.
        /// </summary>
        /// <param name="id">The Vehicle ID</param>
        /// <returns>Vehicle</returns>
        [ResponseType(typeof(VehicleDetailsDto))]
        public async Task<IHttpActionResult> GetVehicle(int id)
        {
            VehicleDetailsDto vehicle = await db.Vehicles.Select(p => new VehicleDetailsDto
            {
                ID = p.ID,
                IncidentID = p.IncidentID,
                PlateNumber = p.PlateNumber
            })
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }
                
        // PUT: api/Vehicles/5
        /// <summary>
        /// Updates Vehicle.
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="vehicle">Modified Vehicle</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.ID)
            {
                return BadRequest();
            }

            db.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        /// <summary>
        /// Creates Vehicle.
        /// </summary>
        /// <param name="vehicleDetailsDto">A new Vehicle</param>
        /// <returns>Created Vehicle</returns>
        [ResponseType(typeof(VehicleDetailsDto))]
        public async Task<IHttpActionResult> PostVehicle(VehicleDetailsDto vehicleDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = new Vehicle()
            {
                ID = vehicleDetailsDto.ID,
                PlateNumber = vehicleDetailsDto.PlateNumber,
                IncidentID = vehicleDetailsDto.IncidentID
            };

            db.Vehicles.Add(vehicle);
            await db.SaveChangesAsync();

            vehicleDetailsDto.ID = vehicle.ID;

            return CreatedAtRoute("DefaultApi", new { id = vehicle.ID }, vehicle);
        }

        // DELETE: api/Vehicles/5
        /// <summary>
        /// Deletes vehicle.
        /// </summary>
        /// <param name="id">The Vehicle ID</param>
        /// <returns>Ok, if deleted successfully</returns>
        [ResponseType(typeof(Vehicle))]
        public async Task<IHttpActionResult> DeleteVehicle(int id)
        {
            Vehicle vehicle = await db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            db.Vehicles.Remove(vehicle);
            await db.SaveChangesAsync();

            var vehicleDetailsDto = new VehicleDetailsDto
            {
                ID = vehicle.ID,
                IncidentID = vehicle.IncidentID,
                PlateNumber = vehicle.PlateNumber
            };

            return Ok(vehicleDetailsDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(int id)
        {
            return db.Vehicles.Count(e => e.ID == id) > 0;
        }
    }
}