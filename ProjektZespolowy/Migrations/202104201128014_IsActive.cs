namespace ProjektZespolowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AirLines", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AirRoutes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Airports", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Passengers", "IsActive", c => c.Boolean(nullable: false));           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Passengers", "IsActive");
            DropColumn("dbo.Airports", "IsActive");
            DropColumn("dbo.AirRoutes", "IsActive");
            DropColumn("dbo.AirLines", "IsActive");
        }
    }
}
