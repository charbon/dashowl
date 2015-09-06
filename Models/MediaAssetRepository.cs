using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class MediaAssetRepository : GenericRepository<DashOwl.DAL.DashOwlContext, MediaAsset>, IMediaAssetRepository
    {

        public List<MediaAsset> GetAllForIncident(int incidentId)
        {
            var query = GetAll().Where(x => x.ID == incidentId);
            return query.ToList<MediaAsset>();
        }

        public MediaAsset GetSingle(int mediaAssetId)
        {
            var query = GetAll().FirstOrDefault(x => x.ID == mediaAssetId);
            return query;
        }
    }
}