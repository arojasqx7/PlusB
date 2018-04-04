namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTicketIdToKPIEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KPIEvaluation", "idTicket", c => c.Int());
            CreateIndex("dbo.KPIEvaluation", "idTicket");
            AddForeignKey("dbo.KPIEvaluation", "idTicket", "dbo.Ticket", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KPIEvaluation", "idTicket", "dbo.Ticket");
            DropIndex("dbo.KPIEvaluation", new[] { "idTicket" });
            DropColumn("dbo.KPIEvaluation", "idTicket");
        }
    }
}
