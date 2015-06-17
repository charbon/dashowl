using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DashOwl.Models
{
    public class MediaAsset
    {
        public int ID { get; set; }
        public int IncidentID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ServerURL { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ExternalURL { get; set; }
        public DateTime CreationDate { get; set; }
        //[ForeignKey("ID")]
        public virtual Incident Incident { get; set; }
    }
}