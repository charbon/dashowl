using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        public int IncidentID { get; set; }

        public string PlateNumber { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }
    }
}