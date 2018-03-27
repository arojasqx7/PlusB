namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileContent = c.Binary(),
                        IdTicket = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.IdTicket, cascadeDelete: true)
                .Index(t => t.IdTicket);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OpenTime = c.Time(nullable: false, precision: 7),
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
                        Creator = c.String(),
                        AssignmentDate = c.DateTime(),
                        AssignmentTime = c.Time(precision: 7),
                        ClosedDate = c.DateTime(),
                        ClosedTime = c.Time(precision: 7),
                        AverageResolution = c.Double(),
                        TotalResolutionHours = c.Double(),
                        AutoAssigned = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.Consultant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IdNumber = c.String(maxLength: 450),
                        Gender = c.String(),
                        Email = c.String(),
                        Pais = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        JobTitle = c.String(),
                        Education = c.String(),
                        HireDate = c.DateTime(nullable: false),
                        IdSLA = c.Int(),
                        SLA_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SLA", t => t.SLA_ID)
                .Index(t => t.IdNumber, unique: true)
                .Index(t => t.SLA_ID);
            
            CreateTable(
                "dbo.SLA",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        Name = c.String(maxLength: 450),
                        Scope = c.String(),
                        ResolutionTimeAverage = c.Double(nullable: false),
                        SupportType = c.String(),
                        PriorityName = c.String(),
                        ResponseTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(maxLength: 450),
                        CompanyId = c.String(maxLength: 450),
                        CompanyDescription = c.String(),
                        ManagerName = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        EmailAddress = c.String(),
                        SupportType = c.String(),
                        InitialDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CompanyName, unique: true)
                .Index(t => t.CompanyId, unique: true);
            
            CreateTable(
                "dbo.Impact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImpactName = c.String(maxLength: 450),
                        ImpactNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ImpactName, unique: true);
            
            CreateTable(
                "dbo.Severity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeverityName = c.String(maxLength: 450),
                        SeverityNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SeverityName, unique: true);
            
            CreateTable(
                "dbo.TaskType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskName = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TaskName, unique: true);
            
            CreateTable(
                "dbo.Technology",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 450),
                        Description = c.String(),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.CustomerSLA",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdCustomer = c.Int(nullable: false),
                        IdSLA = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.IdCustomer, cascadeDelete: true)
                .ForeignKey("dbo.SLA", t => t.IdSLA, cascadeDelete: true)
                .Index(t => t.IdCustomer)
                .Index(t => t.IdSLA);
            
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
                        IdConsultant = c.Int(nullable: false),
                        Consultant_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Consultant", t => t.Consultant_ID)
                .Index(t => t.Consultant_ID);
            
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
            
            CreateTable(
                "dbo.TicketActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        Activity = c.String(),
                        User = c.String(),
                        idTicket = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.idTicket, cascadeDelete: true)
                .Index(t => t.idTicket);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketActivity", "idTicket", "dbo.Ticket");
            DropForeignKey("dbo.Survey", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.KPI", "Consultant_ID", "dbo.Consultant");
            DropForeignKey("dbo.KnowledgeBase", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.CustomerSLA", "IdSLA", "dbo.SLA");
            DropForeignKey("dbo.CustomerSLA", "IdCustomer", "dbo.Customer");
            DropForeignKey("dbo.Attachment", "IdTicket", "dbo.Ticket");
            DropForeignKey("dbo.Ticket", "Id_Technology", "dbo.Technology");
            DropForeignKey("dbo.Ticket", "Id_TaskType", "dbo.TaskType");
            DropForeignKey("dbo.Ticket", "Id_Severity", "dbo.Severity");
            DropForeignKey("dbo.Ticket", "Id_Impact", "dbo.Impact");
            DropForeignKey("dbo.Ticket", "Id_Customer", "dbo.Customer");
            DropForeignKey("dbo.Ticket", "Id_Consultant", "dbo.Consultant");
            DropForeignKey("dbo.Consultant", "SLA_ID", "dbo.SLA");
            DropIndex("dbo.TicketActivity", new[] { "idTicket" });
            DropIndex("dbo.Survey", new[] { "Ticket_Id" });
            DropIndex("dbo.KPI", new[] { "Consultant_ID" });
            DropIndex("dbo.KnowledgeBase", new[] { "Ticket_Id" });
            DropIndex("dbo.CustomerSLA", new[] { "IdSLA" });
            DropIndex("dbo.CustomerSLA", new[] { "IdCustomer" });
            DropIndex("dbo.Technology", new[] { "Name" });
            DropIndex("dbo.TaskType", new[] { "TaskName" });
            DropIndex("dbo.Severity", new[] { "SeverityName" });
            DropIndex("dbo.Impact", new[] { "ImpactName" });
            DropIndex("dbo.Customer", new[] { "CompanyId" });
            DropIndex("dbo.Customer", new[] { "CompanyName" });
            DropIndex("dbo.SLA", new[] { "Name" });
            DropIndex("dbo.Consultant", new[] { "SLA_ID" });
            DropIndex("dbo.Consultant", new[] { "IdNumber" });
            DropIndex("dbo.Ticket", new[] { "Id_Consultant" });
            DropIndex("dbo.Ticket", new[] { "Id_TaskType" });
            DropIndex("dbo.Ticket", new[] { "Id_Impact" });
            DropIndex("dbo.Ticket", new[] { "Id_Severity" });
            DropIndex("dbo.Ticket", new[] { "Id_Technology" });
            DropIndex("dbo.Ticket", new[] { "Id_Customer" });
            DropIndex("dbo.Attachment", new[] { "IdTicket" });
            DropTable("dbo.TicketActivity");
            DropTable("dbo.Survey");
            DropTable("dbo.KPI");
            DropTable("dbo.KnowledgeBase");
            DropTable("dbo.CustomerSLA");
            DropTable("dbo.Technology");
            DropTable("dbo.TaskType");
            DropTable("dbo.Severity");
            DropTable("dbo.Impact");
            DropTable("dbo.Customer");
            DropTable("dbo.SLA");
            DropTable("dbo.Consultant");
            DropTable("dbo.Ticket");
            DropTable("dbo.Attachment");
        }
    }
}
