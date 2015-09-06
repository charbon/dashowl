using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.WebAPI
{
    public class VehicleDto
    {
        public int ID { get; set; }

        public string PlateNumber { get; set; }

        public int IncidentID { get; set; }
    }
}