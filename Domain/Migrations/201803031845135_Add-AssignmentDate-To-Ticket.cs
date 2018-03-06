namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssignmentDateToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "AssignmentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "AssignmentDate");
        }
    }
}
