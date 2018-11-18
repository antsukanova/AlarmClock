namespace AlarmClock.DBAdapter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clock",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LastTriggered = c.DateTime(nullable: false),
                        NextTrigger = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Password = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        Name = c.String(),
                        Surname = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        LastVisited = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clock", "UserId", "dbo.Users");
            DropIndex("dbo.Clock", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Clock");
        }
    }
}
