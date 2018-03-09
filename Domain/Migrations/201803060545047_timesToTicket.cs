namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timesToTicket : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ticket", "AssignmentTime", c => c.Time(precision: 7));
            AlterColumn("dbo.Ticket", "ClosedTime", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "ClosedTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Ticket", "AssignmentTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
