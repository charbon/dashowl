using DashOwl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.WebAPI
{
    public class IncidentDetailsDto
    {
        public int ID { get; set; }

        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        public List<MediaAssetDto> MediaAssets { get; set; }

        public List<VehicleDto> Vehicles { get; set; }
    }
}