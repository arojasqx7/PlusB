namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEscalationReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "EscalationReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "EscalationReason");
        }
    }
}
