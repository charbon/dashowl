using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class IncidentRepository : GenericRepository<DashOwl.DAL.DashOwlContext, Incident>, IIncidentRepository
    {

        public Incident GetSingle(int incidentId)
        {
            int totalCount;
            return Query()
                .Include(i => i.MediaAssets)
                .Include(i => i.Vehicles)
                .OrderBy(i=>i.OrderBy(q=>q.CreationDate))
                .Filter(i=> i.ID == incidentId)
                .GetPage(1, 1000, out totalCount).FirstOrDefault();            
        }

        public IList<Incident> GetAllIncidents()
        {
            return GetAll().ToList<Incident>();
        }
    }
}