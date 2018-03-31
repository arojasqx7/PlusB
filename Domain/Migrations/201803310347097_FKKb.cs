namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKKb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KnowledgeBase", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.KnowledgeBase", new[] { "Ticket_Id" });
            DropColumn("dbo.KnowledgeBase", "IdTicket");
            RenameColumn(table: "dbo.KnowledgeBase", name: "Ticket_Id", newName: "IdTicket");
            AlterColumn("dbo.KnowledgeBase", "IdTicket", c => c.Int(nullable: false));
            CreateIndex("dbo.KnowledgeBase", "IdTicket");
            AddForeignKey("dbo.KnowledgeBase", "IdTicket", "dbo.Ticket", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KnowledgeBase", "IdTicket", "dbo.Ticket");
            DropIndex("dbo.KnowledgeBase", new[] { "IdTicket" });
            AlterColumn("dbo.KnowledgeBase", "IdTicket", c => c.Int());
            RenameColumn(table: "dbo.KnowledgeBase", name: "IdTicket", newName: "Ticket_Id");
            AddColumn("dbo.KnowledgeBase", "IdTicket", c => c.Int(nullable: false));
            CreateIndex("dbo.KnowledgeBase", "Ticket_Id");
            AddForeignKey("dbo.KnowledgeBase", "Ticket_Id", "dbo.Ticket", "Id");
        }
    }
}
