namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketActivityEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        Activity = c.String(),
                        idTicket = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.idTicket, cascadeDelete: true)
                .Index(t => t.idTicket);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketActivity", "idTicket", "dbo.Ticket");
            DropIndex("dbo.TicketActivity", new[] { "idTicket" });
            DropTable("dbo.TicketActivity");
        }
    }
}
