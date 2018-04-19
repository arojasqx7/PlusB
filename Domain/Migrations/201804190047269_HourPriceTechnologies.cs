namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HourPriceTechnologies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Technology", "HourPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Technology", "HourPrice");
        }
    }
}
