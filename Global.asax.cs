using AutoMapper;
using DashOwl.DAL;
using DashOwl.Models;
using DashOwl.WebAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DashOwl
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer(new DashOwlInitializer());
            InitializeAutoMapper();
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
        }

        private void InitializeAutoMapper()
        {
            Mapper.CreateMap<IncidentDetailsDto, Incident>();
            Mapper.CreateMap<MediaAssetDto, MediaAsset>();
            Mapper.CreateMap<VehicleDto, Vehicle>();

            Mapper.CreateMap<Incident, IncidentDetailsDto>();
            Mapper.CreateMap<MediaAsset, MediaAssetDto>();
            Mapper.CreateMap<Vehicle, VehicleDto>();

            //Mapper.CreateMap<Incident, IncidentDetailsDto>().ForMember(x => x.MediaAssets, opt => opt.Ignore());
            //Mapper.CreateMap<Incident, IncidentDetailsDto>().ForMember(x => x.Vehicles, opt => opt.Ignore());

            Mapper.CreateMap<MediaAssetDto, MediaAsset>().ForMember(x => x.Incidents, opt => opt.Ignore());
            Mapper.CreateMap<VehicleDto, Vehicle>().ForMember(x => x.Incidents, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
