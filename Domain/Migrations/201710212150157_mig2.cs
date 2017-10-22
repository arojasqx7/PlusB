namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Technology", "Weight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Technology", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
