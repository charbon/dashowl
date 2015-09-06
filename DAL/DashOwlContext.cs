using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DashOwl.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DashOwl.DAL
{
    public class DashOwlContext : DbContext
    {
        public DashOwlContext()
            : base("DashOwlContext")
        {
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<MediaAsset> MediaAssets { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Incident>()
                .HasMany(s => s.Vehicles)
                .WithMany(i => i.Incidents)
                .Map(cs => cs.MapLeftKey("IncidentID").
                    MapRightKey("VehicleID").
                    ToTable("IncidentVehicle")
                );

            modelBuilder.Entity<Incident>()
                .HasMany(s => s.MediaAssets)
                .WithMany(i => i.Incidents)
                .Map(cs => cs.MapLeftKey("IncidentID").
                    MapRightKey("MediaAssetID").
                    ToTable("IncidentMediaAsset")
                );
        }
    }
}