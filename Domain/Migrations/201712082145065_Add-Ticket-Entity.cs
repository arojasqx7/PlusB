namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Id_Customer = c.Int(nullable: false),
                        ShortDescription = c.String(),
                        LongDescription = c.String(),
                        Environment = c.String(),
                        Id_Technology = c.Int(nullable: false),
                        Id_Severity = c.Int(nullable: false),
                        Id_Impact = c.Int(nullable: false),
                        Id_TaskType = c.Int(nullable: false),
                        Status = c.String(),
                        Id_Consultant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consultant", t => t.Id_Consultant, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.Id_Customer, cascadeDelete: true)
                .ForeignKey("dbo.Impact", t => t.Id_Impact, cascadeDelete: true)
                .ForeignKey("dbo.Severity", t => t.Id_Severity, cascadeDelete: true)
                .ForeignKey("dbo.TaskType", t => t.Id_TaskType, cascadeDelete: true)
                .ForeignKey("dbo.Technology", t => t.Id_Technology, cascadeDelete: true)
                .Index(t => t.Id_Customer)
                .Index(t => t.Id_Technology)
                .Index(t => t.Id_Severity)
                .Index(t => t.Id_Impact)
                .Index(t => t.Id_TaskType)
                .Index(t => t.Id_Consultant);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "Id_Technology", "dbo.Technology");
            DropForeignKey("dbo.Ticket", "Id_TaskType", "dbo.TaskType");
            DropForeignKey("dbo.Ticket", "Id_Severity", "dbo.Severity");
            DropForeignKey("dbo.Ticket", "Id_Impact", "dbo.Impact");
            DropForeignKey("dbo.Ticket", "Id_Customer", "dbo.Customer");
            DropForeignKey("dbo.Ticket", "Id_Consultant", "dbo.Consultant");
            DropIndex("dbo.Ticket", new[] { "Id_Consultant" });
            DropIndex("dbo.Ticket", new[] { "Id_TaskType" });
            DropIndex("dbo.Ticket", new[] { "Id_Impact" });
            DropIndex("dbo.Ticket", new[] { "Id_Severity" });
            DropIndex("dbo.Ticket", new[] { "Id_Technology" });
            DropIndex("dbo.Ticket", new[] { "Id_Customer" });
            DropTable("dbo.Ticket");
        }
    }
}
