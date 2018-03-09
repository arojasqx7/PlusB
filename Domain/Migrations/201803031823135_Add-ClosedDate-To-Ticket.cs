namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClosedDateToTicket : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ticket", "ClosedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "ClosedDate", c => c.DateTime(nullable: false));
        }
    }
}
