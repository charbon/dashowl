using DashOwl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;

namespace DashOwl.DAL
{
    public class DashOwlInitializer : DropCreateDatabaseIfModelChanges<DashOwlContext>
    {
       
        protected override void Seed(DashOwlContext context)
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle{PlateNumber="BC3522CH"},
                new Vehicle{PlateNumber="BC7391CB"}
            };

            vehicles.ForEach(s => context.Vehicles.Add(s));
            context.SaveChanges();

            var incidents = new List<Incident>
            {
                new Incident{Description="Some dummy description 1", CreationDate=DateTime.Parse("2015-05-01"), Vehicles= new List<Vehicle>()},
                new Incident{Description="Some dummy description 2", CreationDate=DateTime.Parse("2015-04-01"), Vehicles= new List<Vehicle>()},
                new Incident{Description="Some dummy description 3", CreationDate=DateTime.Parse("2015-02-17"), Vehicles= new List<Vehicle>()}
            };

            incidents.ForEach(s => context.Incidents.Add(s));
            context.SaveChanges();

            var mediaAssets = new List<MediaAsset>
            {
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"),
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 1").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/1.jpg")
                },
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"),
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 1").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/2.jpg")
                },                
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"),
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 1").ID,
                    ExternalURL="https://auto.imgsmail.ru/content/documents/in_text_images/0/0/00141e8e5d39829f430eacdb0bb7ae94_small.jpeg", 
                    ServerURL=""
                },
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"), 
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 2").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/3.jpg")
                },
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"),
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 2").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/4.jpg")
                },               
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"),
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 2").ID,
                    ExternalURL="https://auto.imgsmail.ru/content/documents/in_text_images/a/8/a8bbf6779c29a12cddc502da548f7769.jpeg", 
                    ServerURL=""
                },
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"), 
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 3").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/6.jpg")
                },
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"), 
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 3").ID,
                    ExternalURL="", 
                    ServerURL=VirtualPathUtility.ToAbsolute("~/MediaStorage/7.jpg")
                }, 
                new MediaAsset{
                    CreationDate=DateTime.Parse("2014-12-31"), 
                    IncidentID = incidents.Single(i => i.Description == "Some dummy description 2").ID,
                    ExternalURL="https://cars.imgsmail.ru/pic180x110/catalogue/generations/5/4/54034e2b9824b0d3fa7222418c130311_240x150.png", 
                    ServerURL=""
                }
            };

            mediaAssets.ForEach(s => context.MediaAssets.Add(s));
            context.SaveChanges();

            AddOrUpdateVehicle(context, "Some dummy description 1", "BC3522CH");
            AddOrUpdateVehicle(context, "Some dummy description 2", "BC7391CB");
            AddOrUpdateVehicle(context, "Some dummy description 2", "BC3522CH");
            AddOrUpdateVehicle(context, "Some dummy description 3", "BC7391CB");
        }

        private void AddOrUpdateVehicle(DashOwlContext context, string incidentDescription, string vehiclePlateNumber)
        {
            var incident = context.Incidents.SingleOrDefault(i => i.Description == incidentDescription);
            var vehicle = context.Vehicles.SingleOrDefault(v => v.PlateNumber == vehiclePlateNumber);
            if (vehicle != null)
                incident.Vehicles.Add(context.Vehicles.Single(v => v.PlateNumber == vehiclePlateNumber));
        }
    }
}