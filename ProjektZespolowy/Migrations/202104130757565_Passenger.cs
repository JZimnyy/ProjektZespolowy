namespace ProjektZespolowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Passenger : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        PassengerId = c.Int(nullable: false, identity: true),
                        PublicId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        PESEL = c.String(nullable: false, maxLength: 11),
                        DocumentSerial = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PassengerId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
                
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Passengers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Passengers", new[] { "User_Id" });
            DropTable("dbo.Passengers");
        }
    }
}
