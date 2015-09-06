using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class Incident
    {
        public int ID { get; set; }

        public DateTime CreationDate { get; set; }        

        public string Description { get; set; }

        public virtual ICollection<MediaAsset> MediaAssets { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}