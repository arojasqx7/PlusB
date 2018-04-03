namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Survey", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.Survey", new[] { "Ticket_Id" });
            DropColumn("dbo.Survey", "IdTicket");
            RenameColumn(table: "dbo.Survey", name: "Ticket_Id", newName: "idTicket");
            AddColumn("dbo.Survey", "Answer1", c => c.Int(nullable: false));
            AddColumn("dbo.Survey", "Answer2", c => c.Int(nullable: false));
            AddColumn("dbo.Survey", "Answer3", c => c.String());
            AlterColumn("dbo.Survey", "idTicket", c => c.Int(nullable: false));
            CreateIndex("dbo.Survey", "idTicket");
            AddForeignKey("dbo.Survey", "idTicket", "dbo.Ticket", "Id", cascadeDelete: true);
            DropColumn("dbo.Survey", "Question1");
            DropColumn("dbo.Survey", "Question2");
            DropColumn("dbo.Survey", "Question3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Survey", "Question3", c => c.String());
            AddColumn("dbo.Survey", "Question2", c => c.Int(nullable: false));
            AddColumn("dbo.Survey", "Question1", c => c.Int(nullable: false));
            DropForeignKey("dbo.Survey", "idTicket", "dbo.Ticket");
            DropIndex("dbo.Survey", new[] { "idTicket" });
            AlterColumn("dbo.Survey", "idTicket", c => c.Int());
            DropColumn("dbo.Survey", "Answer3");
            DropColumn("dbo.Survey", "Answer2");
            DropColumn("dbo.Survey", "Answer1");
            RenameColumn(table: "dbo.Survey", name: "idTicket", newName: "Ticket_Id");
            AddColumn("dbo.Survey", "IdTicket", c => c.Int(nullable: false));
            CreateIndex("dbo.Survey", "Ticket_Id");
            AddForeignKey("dbo.Survey", "Ticket_Id", "dbo.Ticket", "Id");
        }
    }
}
