using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.WebAPI
{
    public class MediaAssetDto
    {
        public int ID { get; set; }
        
        public string ServerURL { get; set; }
        
        public string ExternalURL { get; set; }

        public int IncidentID { get; set; }

        public DateTime CreationDate { get; set; }
    }
}