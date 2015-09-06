using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public interface IMediaAssetRepository : IGenericRepository<MediaAsset>
    {
        List<MediaAsset> GetAllForIncident(int incidentId);

        MediaAsset GetSingle(int mediaAssetId);
    }
}