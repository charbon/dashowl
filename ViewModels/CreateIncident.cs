using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashOwl.ViewModels
{
    public class CreateIncident
    {
        public int ID { get; set; }
        public DateTime CreationDate { get; set; }
        public string ExternalURL { get; set; }
        public string ServerPath { get; set; }
        public string Description { get; set; }
    }
}