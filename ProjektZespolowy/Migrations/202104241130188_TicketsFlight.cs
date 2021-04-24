namespace ProjektZespolowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketsFlight : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        FlightId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        AirRouteId = c.Int(nullable: false),
                        NumberOfFreeSeats = c.Int(nullable: false),
                        DepartureDate = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.FlightId)
                .ForeignKey("dbo.AirRoutes", t => t.AirRouteId, cascadeDelete: true)
                .Index(t => t.AirRouteId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        PassengerId = c.Int(nullable: false),
                        FlightId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.Flights", t => t.FlightId, cascadeDelete: true)
                .ForeignKey("dbo.Passengers", t => t.PassengerId, cascadeDelete: true)
                .Index(t => t.PassengerId)
                .Index(t => t.FlightId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "PassengerId", "dbo.Passengers");
            DropForeignKey("dbo.Tickets", "FlightId", "dbo.Flights");
            DropForeignKey("dbo.Flights", "AirRouteId", "dbo.AirRoutes");
            DropIndex("dbo.Tickets", new[] { "FlightId" });
            DropIndex("dbo.Tickets", new[] { "PassengerId" });
            DropIndex("dbo.Flights", new[] { "AirRouteId" });
            DropTable("dbo.Tickets");
            DropTable("dbo.Flights");
        }
    }
}
