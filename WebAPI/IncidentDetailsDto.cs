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

        public List<MediaAssetDetailsDto> MediaAssets { get; set; }

        public List<VehicleDetailsDto> Vehicles { get; set; }
    }
}