namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResolutionHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "TotalResolutionHours", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "TotalResolutionHours");
        }
    }
}
