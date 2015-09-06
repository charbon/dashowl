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
using AutoMapper;

namespace DashOwl.WebAPI
{
    /// <summary>
    /// Vehicle entity API controller
    /// </summary>
    public class VehiclesController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();
        private readonly IVehicleRepository _vehicleRepo;

        public VehiclesController(IVehicleRepository repository)
        {
            this._vehicleRepo = repository;
        }

        // GET: api/Incidents/1/Vehicles
        /// <summary>
        /// Gets a list of Vehicles related to Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>A list of Vehicles</returns>
        [Route("api/incidents/{id}/vehicles")]
        public async Task<IHttpActionResult> GetVehicles(int id)
        {
            List<Vehicle> vehicles = _vehicleRepo.GetAllForIncident(id);
            IList<VehicleDto> vehiclesDtoList = Mapper.Map<IList<Vehicle>, IList<VehicleDto>>(vehicles);
            
            if (vehiclesDtoList == null)
            {
                return NotFound();
            }

            return Ok(vehiclesDtoList);
        }

        // GET: api/Vehicles/5
        /// <summary>
        /// Gets Vehicle by ID.
        /// </summary>
        /// <param name="id">The Vehicle ID</param>
        /// <returns>Vehicle</returns>
        [ResponseType(typeof(VehicleDto))]
        public async Task<IHttpActionResult> GetVehicle(int id)
        {
            Vehicle vehicle = _vehicleRepo.GetSingle(id);
            var vehicleDto = Mapper.Map<Vehicle, VehicleDto>(vehicle);

            if (vehicleDto == null)
            {
                return NotFound();
            }

            return Ok(vehicleDto);
        }
                
        // PUT: api/Vehicles
        /// <summary>
        /// Updates Vehicle.
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="vehicleDto">Modified VehicleDto. JSON sample: {"ID":1,"PlateNumber":"BC3555CH","IncidentID":0}</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVehicle([FromBody]VehicleDto vehicleDto)
        {
            Vehicle vehicle = Mapper.Map<VehicleDto, Vehicle>(vehicleDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _vehicleRepo.Edit(vehicle);
            _vehicleRepo.Save();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Vehicles
        /// <summary>
        /// Creates Vehicle.
        /// </summary>
        /// <param name="vehicleDto">A new VehicleDto. JSON sample: {"ID":1,"PlateNumber":"BC3555CH","IncidentID":0}</param>
        /// <returns>Created Vehicle</returns>
        [ResponseType(typeof(VehicleDto))]
        public async Task<IHttpActionResult> PostVehicle(VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = Mapper.Map<VehicleDto, Vehicle>(vehicleDto);

            _vehicleRepo.Add(vehicle);
            _vehicleRepo.Save();

            vehicleDto = Mapper.Map<Vehicle, VehicleDto>(vehicle);

            return Ok(vehicleDto);
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
            Vehicle vehicle = _vehicleRepo.GetSingle(id);
            var vehicleDto = Mapper.Map<Vehicle, VehicleDto>(vehicle);

            if (vehicleDto == null)
            {
                return NotFound();
            }

            _vehicleRepo.Delete(vehicle);
            _vehicleRepo.Save();

            return Ok(vehicleDto);
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