namespace DashOwl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incident",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MediaAsset",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IncidentID = c.Int(nullable: false),
                        ServerURL = c.String(),
                        ExternalURL = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Incident", t => t.IncidentID, cascadeDelete: true)
                .Index(t => t.IncidentID);
            
            CreateTable(
                "dbo.Vehicle",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlateNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IncidentVehicle",
                c => new
                    {
                        IncidentID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IncidentID, t.VehicleID })
                .ForeignKey("dbo.Incident", t => t.IncidentID, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleID, cascadeDelete: true)
                .Index(t => t.IncidentID)
                .Index(t => t.VehicleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncidentVehicle", "VehicleID", "dbo.Vehicle");
            DropForeignKey("dbo.IncidentVehicle", "IncidentID", "dbo.Incident");
            DropForeignKey("dbo.MediaAsset", "IncidentID", "dbo.Incident");
            DropIndex("dbo.IncidentVehicle", new[] { "VehicleID" });
            DropIndex("dbo.IncidentVehicle", new[] { "IncidentID" });
            DropIndex("dbo.MediaAsset", new[] { "IncidentID" });
            DropTable("dbo.IncidentVehicle");
            DropTable("dbo.Vehicle");
            DropTable("dbo.MediaAsset");
            DropTable("dbo.Incident");
        }
    }
}
