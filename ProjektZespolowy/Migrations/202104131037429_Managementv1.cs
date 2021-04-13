namespace ProjektZespolowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Managementv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirLines",
                c => new
                    {
                        AirLineId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        LinkToPage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AirLineId);
            
            CreateTable(
                "dbo.AirRoutes",
                c => new
                    {
                        AirRouteId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        AirLineId = c.Int(nullable: false),
                        StartAirportCode = c.String(nullable: false),
                        FinishAirportCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AirRouteId)
                .ForeignKey("dbo.AirLines", t => t.AirLineId, cascadeDelete: true)
                .Index(t => t.AirLineId);
            
            CreateTable(
                "dbo.Airports",
                c => new
                    {
                        AirportId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false, maxLength: 3),
                    })
                .PrimaryKey(t => t.AirportId)
                .Index(t => t.Code, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirRoutes", "AirLineId", "dbo.AirLines");
            DropIndex("dbo.Airports", new[] { "Code" });
            DropIndex("dbo.AirRoutes", new[] { "AirLineId" });
            DropTable("dbo.Airports");
            DropTable("dbo.AirRoutes");
            DropTable("dbo.AirLines");
        }
    }
}
