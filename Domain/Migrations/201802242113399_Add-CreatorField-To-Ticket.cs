namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatorFieldToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "Creator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "Creator");
        }
    }
}
