namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueCustomerName : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customer", "CompanyName", unique: true);
            CreateIndex("dbo.Customer", "CompanyId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customer", new[] { "CompanyId" });
            DropIndex("dbo.Customer", new[] { "CompanyName" });
        }
    }
}
