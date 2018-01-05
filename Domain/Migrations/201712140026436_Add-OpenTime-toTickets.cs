namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpenTimetoTickets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "OpenTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Ticket", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "Date", c => c.DateTime());
            DropColumn("dbo.Ticket", "OpenTime");
        }
    }
}
