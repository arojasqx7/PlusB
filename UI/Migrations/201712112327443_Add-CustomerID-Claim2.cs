namespace UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerIDClaim2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CustomerID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CustomerID", c => c.Int());
        }
    }
}
