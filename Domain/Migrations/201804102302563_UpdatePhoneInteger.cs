namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePhoneInteger : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Consultant", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Customer", "PhoneNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Consultant", "PhoneNumber", c => c.String());
        }
    }
}
