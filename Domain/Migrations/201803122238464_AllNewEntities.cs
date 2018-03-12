namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllNewEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerSLA",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdCustomer = c.Int(nullable: false),
                        IdSLA = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                        SLA_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.Customer_Id)
                .ForeignKey("dbo.SLA", t => t.SLA_ID)
                .Index(t => t.Customer_Id)
                .Index(t => t.SLA_ID);
            
            CreateTable(
                "dbo.KnowledgeBase",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        Solution = c.String(),
                        SpecialDetails = c.String(),
                        IdTicket = c.Int(nullable: false),
                        Ticket_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ticket", t => t.Ticket_Id)
                .Index(t => t.Ticket_Id);
            
            CreateTable(
                "dbo.KPI",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Objective = c.String(),
                        FormulaValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Survey",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        Question1 = c.Int(nullable: false),
                        Question2 = c.Int(nullable: false),
                        Question3 = c.String(),
                        Comments = c.String(),
                        DateAnswered = c.DateTime(),
                        IdTicket = c.Int(nullable: false),
                        Ticket_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ticket", t => t.Ticket_Id)
                .Index(t => t.Ticket_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Survey", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.KnowledgeBase", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.CustomerSLA", "SLA_ID", "dbo.SLA");
            DropForeignKey("dbo.CustomerSLA", "Customer_Id", "dbo.Customer");
            DropIndex("dbo.Survey", new[] { "Ticket_Id" });
            DropIndex("dbo.KnowledgeBase", new[] { "Ticket_Id" });
            DropIndex("dbo.CustomerSLA", new[] { "SLA_ID" });
            DropIndex("dbo.CustomerSLA", new[] { "Customer_Id" });
            DropTable("dbo.Survey");
            DropTable("dbo.KPI");
            DropTable("dbo.KnowledgeBase");
            DropTable("dbo.CustomerSLA");
        }
    }
}
