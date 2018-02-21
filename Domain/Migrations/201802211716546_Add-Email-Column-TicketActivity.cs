namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailColumnTicketActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketActivity", "User", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketActivity", "User");
        }
    }
}
