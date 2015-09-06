using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        List<Vehicle> GetAllForIncident(int incidentId);

        Vehicle GetSingle(int vehicleId);
    }
}