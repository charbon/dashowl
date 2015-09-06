using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public interface IIncidentRepository : IGenericRepository<Incident>
    {
        IList<Incident> GetAllIncidents();

        Incident GetSingle(int incidentId);
    }
}