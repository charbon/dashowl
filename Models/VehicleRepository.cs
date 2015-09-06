using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class VehicleRepository : GenericRepository<DashOwl.DAL.DashOwlContext, Vehicle>, IVehicleRepository
    {
        public List<Vehicle> GetAllForIncident(int incidentId)
        {
            var query = GetAll().Where(x => x.ID == incidentId);
            return query.ToList<Vehicle>();
        }

        public Vehicle GetSingle(int vehicleId)
        {
            var query = GetAll().FirstOrDefault(x => x.ID == vehicleId);
            return query;
        }
    }
}